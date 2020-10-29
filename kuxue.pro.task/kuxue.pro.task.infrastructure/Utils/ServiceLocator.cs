using Microsoft.Extensions.DependencyInjection;
using System;

namespace kuxue.pro.task.infrastructure.Utils
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void SetServiceProvider(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (ServiceProvider == null)
            {
                ServiceProvider = services.BuildServiceProvider();
            }
        }
    }
}