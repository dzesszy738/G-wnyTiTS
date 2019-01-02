using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Logowanie.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Logowanie.Models;
using Microsoft.AspNetCore.Http;
using System.Web.Http;


using Logowanie.Data;



namespace Logowanie
{
    public class Program
    {


        public static void Main(string[] args)
        {

            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    Seed.CreateUserRoles(serviceProvider, configuration).Wait();

                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "Error");
                }


            }
            host.Run();



        }
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
        }
    }




