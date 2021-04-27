using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace PSN
{
    public class Startup
    {
        private Storage _storage;
        private Queue _queue;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _storage = new Storage(Configuration["Storage:Path"]);
            _queue = new Queue();
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
                    using var requestStream = context.Request.Body;
                    var (imageId, uploadedOn) = await _storage.Save(requestStream);
                    _queue.Publish(new ImageUploadedMessage
                    {
                        ImageId = imageId,
                        UploadedOn = uploadedOn
                    });

                    var response = JsonConvert.SerializeObject(new { id = imageId });
                    await context.Response.WriteAsync(response);
                });
            });
        }
    }
}
