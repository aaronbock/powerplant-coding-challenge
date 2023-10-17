using Gems.Power.Domain.Contracts;
using Gems.Power.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gems.Power.Domain
{
    public static class Startup
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddTransient<ICalculatorService, CalculatorService>();
        }
    }
}
