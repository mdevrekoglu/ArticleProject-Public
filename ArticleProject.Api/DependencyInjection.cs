using ArticleProject.Api.Application.Behaviors;
using ArticleProject.Api.Application.Jobs;
using ArticleProject.Data.ArticleProjectDbContext;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace ArticleProject.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Fluent Validators
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Adds additional calls
            services.AddProblemDetails();

            // Identity setup with Entity Framework
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.SignIn.RequireConfirmedEmail = true;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<ArticleProjectContext>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });

            // To access uri
            services.AddHttpContextAccessor();

            // To add all services and queries
            var programAssembly = Assembly.GetExecutingAssembly();

            var baseServiceTypes = programAssembly
                .GetTypes()
                .Where(p => typeof(IBaseService).IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);

            foreach (var implementationType in baseServiceTypes)
            {
                var serviceInterfaces = implementationType.GetInterfaces()
                    .Where(i => i != typeof(IBaseService)
                                && !typeof(IBaseSingletonService).IsAssignableFrom(i)
                                && !typeof(IBaseScopedService).IsAssignableFrom(i)
                                && !typeof(IBaseTransientService).IsAssignableFrom(i));

                var serviceType = serviceInterfaces.FirstOrDefault() ?? implementationType;

                if (typeof(IBaseSingletonService).IsAssignableFrom(implementationType))
                {
                    services.AddSingleton(serviceType, implementationType);
                }
                else if (typeof(IBaseScopedService).IsAssignableFrom(implementationType))
                {
                    services.AddScoped(serviceType, implementationType);
                }
                else if (typeof(IBaseTransientService).IsAssignableFrom(implementationType))
                {
                    services.AddTransient(serviceType, implementationType);
                }
            }

            var queryTypes = programAssembly.GetTypes().Where(p => typeof(IQuery).IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var queryType in queryTypes)
            {
                services.AddScoped(queryType, queryType);
            }

            // Add mediatr and its configurations
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
            });

            // Add transaction behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // Add cron jobs
            var scheduledJobs = configuration.GetSection("ScheduledJobs").Get<Dictionary<string, JobConfig>>();
            if (scheduledJobs != null)
            {
                if (scheduledJobs["FetchScopusDataJob"] != null && scheduledJobs["FetchScopusDataJob"].Enabled)
                {
                    services.AddCronJob<FetchScopusDataJob>(cfg => cfg.CronExpression = scheduledJobs["FetchScopusDataJob"].CronExpression);
                }
            }

            return services;
        }
    }
}