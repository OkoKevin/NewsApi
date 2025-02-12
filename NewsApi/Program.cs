using Microsoft.AspNetCore.Mvc;

namespace NewsApi
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            // Configure the HTTP request pipeline.
            var app = builder.Build();
            startup.Configure(app, app.Environment);
            app.Run();
        }
    }
}