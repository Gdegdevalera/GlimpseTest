using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PSN.Infrastructure;
using PSN.Services;
using System;
using System.IO;

namespace PSN
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMqClient, MqClient>();
            services.AddSingleton<IStorage, Storage>();
            services.AddSingleton<RequestHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/", async context =>
                {
                    var handler = context.RequestServices.GetRequiredService<RequestHandler>();

                    using var requestStream = context.Request.Body;
                    var message = await handler.Handle(requestStream);

                    Console.WriteLine($"Image id:{message.ImageId} was saved on {message.UploadedOn}");

                    var response = JsonConvert.SerializeObject(message);
                    await context.Response.WriteAsync(response);
                });
            });
        }
    }
}
