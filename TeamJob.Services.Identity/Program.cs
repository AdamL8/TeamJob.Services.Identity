using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Events.External;
using Teamjob.Services.Identity.Filters;
using Teamjob.Services.Identity.Requests.Validations;

namespace Teamjob.Services.Identity
{
    public class Program
    {
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count", "Content-Type" };
        public static Task Main(string[] args)
                    => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddTransient(typeof(IPasswordHasher<User>), typeof(PasswordHasher<User>));
                    services.AddDistributedMemoryCache();

                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", builder =>
                                builder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader());
                    });

                    services.AddMvc(options =>
                    {
                        options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                        options.Filters.Add(typeof(ValidatorActionFilter));

                    }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>());

                    services.Configure<ApiBehaviorOptions>(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });

                    services.AddConvey()
                        .AddWebApi()
                        .AddMongo()
                        .AddMongoRepository<User,         Guid>("Users")
                        .AddMongoRepository<RefreshToken, Guid>("RefreshTokens")
                        .AddCommandHandlers()
                        .AddEventHandlers()
                        .AddQueryHandlers()
                        .AddInMemoryCommandDispatcher()
                        .AddInMemoryEventDispatcher()
                        .AddInMemoryQueryDispatcher()
                        .AddJwt()
                        .AddRabbitMq()
                        .Build();
                }) 
                    .Configure(app => app
                        .UseAuthentication()
                        .UseConvey()
                        .UseErrorHandler()
                        .UseCors("CorsPolicy")
                        .UseAllForwardedHeaders()
                        .UseRouting()
                        .UseEndpoints(r => r.MapControllers())
                        .UseRabbitMq()
                        .SubscribeEvent<ProfileDeleted>())
                    .UseLogging();
            });
    }
}
