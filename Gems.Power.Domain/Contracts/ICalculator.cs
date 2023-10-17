using Gems.Power.Domain.Models;

namespace Gems.Power.Domain.Contracts
{
    public interface ICalculatorService
    {
        ScenarioResult Simulate(Scenario scenario);
    }
}