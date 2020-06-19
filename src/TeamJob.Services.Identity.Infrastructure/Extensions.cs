using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Security;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TeamJob.Services.Identity.Application.Commands;
using TeamJob.Services.Identity.Application.Events.External;
using TeamJob.Services.Identity.Application.Services;
using TeamJob.Services.Identity.Application.Services.Identity;
using TeamJob.Services.Identity.Core.Repositories;
using TeamJob.Services.Identity.Infrastructure.Auth;
using TeamJob.Services.Identity.Infrastructure.Contexts;
using TeamJob.Services.Identity.Infrastructure.Decorators;
using TeamJob.Services.Identity.Infrastructure.Exceptions;
using TeamJob.Services.Identity.Infrastructure.Mongo;
using TeamJob.Services.Identity.Infrastructure.Mongo.Documents;
using TeamJob.Services.Identity.Infrastructure.Mongo.Repositories;
using TeamJob.Services.Identity.Infrastructure.Services;

namespace TeamJob.Services.Identity.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddSingleton<IRng, Rng>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            // First check if env vars exist
            string mongoConnectionString = Environment.GetEnvironmentVariable("IDENTITY_DATABASE_CONNECTION_STRING");

            if (mongoConnectionString != null)
            {
                // Set the mongo parameters
                MongoDbOptions mongoOptions = new MongoDbOptions
                {
                    ConnectionString = mongoConnectionString,
                    Database = "identity-service",
                    Seed = false
                };

                return builder
                    .AddErrorHandler<ExceptionToResponseMapper>()
                    .AddQueryHandlers()
                    .AddInMemoryQueryDispatcher()
                    .AddJwt()
                    .AddHttpClient()
                    .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                    .AddRabbitMq()
                    .AddMessageOutbox(o => o.AddMongo())
                    .AddMongo(mongoOptions)
                    .AddMetrics()
                    .AddMongoRepository<RefreshTokenDocument, Guid>("refreshTokens")
                    .AddMongoRepository<UserDocument,         Guid>("users")
                    .AddWebApiSwaggerDocs()
                    .AddSecurity();
            } else {
                return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddJwt()
                .AddHttpClient()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddRabbitMq()
                .AddMessageOutbox(o => o.AddMongo())
                .AddMongo()
                .AddMetrics()
                .AddMongoRepository<RefreshTokenDocument, Guid>("refreshTokens")
                .AddMongoRepository<UserDocument,         Guid>("users")
                .AddWebApiSwaggerDocs()
                .AddSecurity();
            }
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseConvey()
                .UseAccessTokenValidator()
                .UseMongo()
                .UseMetrics()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeEvent<ProfileDeleted>();

            return app;
        }

        public static async Task<Guid> AuthenticateUsingJwtAsync(this HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            return authentication.Succeeded ? Guid.Parse(authentication.Principal.Identity.Name) : Guid.Empty;
        }

        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
            => accessor.HttpContext?.Request.Headers.TryGetValue("Correlation-Context", out var json) is true
                ? JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault())
                : null;

        internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                ? null
                : new Dictionary<string, object>
                {
                    [sagaHeader] = saga
                };
        }

        internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
    }
}
