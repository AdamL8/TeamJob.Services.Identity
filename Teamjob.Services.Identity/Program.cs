using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Teamjob.Services.Identity.Domain;
using Teamjob.Services.Identity.Filters;

namespace Teamjob.Services.Identity
{
    public class Program
    {
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count" };
        public static Task Main(string[] args)
                    => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddTransient(typeof(IPasswordHasher<User>), typeof(PasswordHasher<User>));
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", builder =>
                                builder
                                //builder.AllowAnyOrigin()
                                //       .AllowAnyMethod()
                                //       .AllowAnyHeader()
                                       .AllowCredentials()
                                       .WithExposedHeaders(Headers));
                    });
                    services.AddDistributedMemoryCache();
                    services.AddMvc(options =>
                    {
                        options.Filters.Add(typeof(CustomExceptionFilterAttribute));

                    }); 

                    services.AddConvey()
                        .AddWebApi()
                        .AddCommandHandlers()
                        .AddInMemoryCommandDispatcher()
                        .AddMongo()
                        .AddMongoRepository<User,         Guid>("Users")
                        .AddMongoRepository<RefreshToken, Guid>("RefreshTokens")
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
                        .UseInitializers()
                        .UseErrorHandler()
                        .UseCors("CorsPolicy")
                        .UseAllForwardedHeaders()
                        .UseRouting()
                        .UseEndpoints(r => r.MapControllers())
                        .UseRabbitMq())
                    .UseLogging();
            });
    }
}
