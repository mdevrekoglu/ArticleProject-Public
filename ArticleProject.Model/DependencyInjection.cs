using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ArticleProject.Model
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddModel(this IServiceCollection services)
        {
            // Add all validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}