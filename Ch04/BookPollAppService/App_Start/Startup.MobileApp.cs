using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using BookPollAppService.DataObjects;
using BookPollAppService.Models;
using Owin;

namespace BookPollAppService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new BookPollAppInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<BookPollAppContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class BookPollAppInitializer : CreateDatabaseIfNotExists<BookPollAppContext>
    {
        protected override void Seed(BookPollAppContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            List<PollQuestion> Questions = new List<PollQuestion>
            {
                new PollQuestion { Id = Guid.NewGuid().ToString(), Text = "What book would you like to read?",
                    Answers = "Beginning Entity Framework Core 2.0|Beginning Windows Mixed Reality Programming|Business in Real-Time, Using Azure IoT|Cyber Security on Azure|Angular 5 and .NET Core 2" },
                new PollQuestion { Id = Guid.NewGuid().ToString(), Text = "What is your favorite book category?",
                    Answers = "Apple and iPS|Programming|Machine Learning|Mobile|Microsoft and .NET" },
            };
            foreach (PollQuestion question in Questions)
            {
                context.Set<PollQuestion>().Add(question);
            }
            context.SaveChanges();
            base.Seed(context);
        }
    }
}

