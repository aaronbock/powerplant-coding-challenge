using Gems.Power.Domain.Contracts;
using Gems.Power.Domain.Models;

namespace Gems.Power.Domain.Services
{
    public class CalculatorService : ICalculatorService
    {
        public ScenarioResult Simulate(Scenario scenario)
        {
            // null check
            if (scenario == null)
            {
                return ScenarioResult.AsError("Scenario needs to be provided in order to perform calculation");
            }

            // first get the load to be able to check when it´s been reached
            var targetLoad = scenario.Load;

            // todo move validation to another class?
            if (targetLoad <= 0)
            {
                return ScenarioResult.AsError("Load needs to be greater than zero");
            }

            var powerPlants = new Dictionary<string, double>();

            foreach (var powerPlant in scenario.PowerPlants.OrderBy(x => x.CostOfMegawatt).ThenByDescending(x => x.Pmax))
            {
                // todo check with biz
                // Do we switch off/on wind powerplants since they are free in the (unlikely) event
                // of the load in the system being less than the wind powerplants?

                // here we assume that we will produce all the energy available in wind power plants
                // even if it surpasses the needs (load)
                if (powerPlant.OperatesWith.IsWind)
                {
                    powerPlants.Add(powerPlant.Name!, powerPlant.AllProductionAvailable);
                    targetLoad -= powerPlant.AllProductionAvailable;

                    continue;
                }

                // Get the next power plant available and check how much we can extract from there
                var availableProduction = powerPlant.CalculateAvailableProduction(targetLoad);

                // add plants even if there is no power generated
                // it brings more visibility to add all plants in the response
                // but it´s open to debate wheter is necessary or not
                powerPlants.Add(powerPlant.Name!, availableProduction);
                targetLoad -= availableProduction;
            }

            return ScenarioResult.AsSuccess(powerPlants);
        }
    }
}
