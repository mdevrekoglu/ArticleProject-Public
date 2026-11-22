using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace ArticleProject.Resources
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddResources(this IServiceCollection services)
        {
            // Add localization services
            services.AddLocalization();

            return services;
        }

        public static void UseResources(this IApplicationBuilder app)
        {
            // Add localization to the request pipeline
            var defaultCulture = "en"; // Default to English
            var supportedCultures = new[] { "en", "tr" };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray(),
                SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray()
            });
        }
    }
}