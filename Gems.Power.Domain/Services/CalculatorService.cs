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

            if (scenario.PowerPlants == null)
            {
                return ScenarioResult.AsError("Power plants need to be provided in order to perform calculation");
            }

            // todo move validation to another class?
            if (scenario.Load <= 0)
            {
                return ScenarioResult.AsError("Load needs to be greater than zero");
            }

            // first get the load to be able to check when it´s been reached
            var remaningLoad = scenario.Load;

            var powerPlants = new Dictionary<string, double>();

            var copyOfPowerPlants = scenario.PowerPlants.OrderBy(x => x.CostOfMegawatt).ToList();

            for (int i = 0; i < copyOfPowerPlants.Count; i++)
            {
                var currentPowerPlant = copyOfPowerPlants[i];
                // todo check with biz
                // Do we switch off/on wind powerplants since they are free in the (unlikely) event
                // of the load in the system being less than the wind powerplants?

                // here we assume that we will produce all the energy available in wind power plants
                // even if it surpasses the needs (load)
                if (currentPowerPlant.OperatesWith.IsWind)
                {
                    powerPlants.Add(currentPowerPlant.Name!, currentPowerPlant.AllProductionAvailable.Value);
                    remaningLoad -= currentPowerPlant.AllProductionAvailable.Value;

                    continue;
                }

                // Get the next power plant available and check how much we can extract from there
                var minimumAllowedReached = currentPowerPlant.TryToProduce(remaningLoad, out double output, out double minimumAllowed);

                if (minimumAllowedReached)
                {
                    // add plants even if there is no power generated
                    // it brings more visibility to add all plants in the response
                    // but it´s open to debate wheter is necessary or not
                    powerPlants.Add(currentPowerPlant.Name!, output);
                    remaningLoad -= output;
                }
                else
                {
                    var removeFromPrevious = minimumAllowed - remaningLoad;

                    var success = TryToRemoveDifferenceFromPrevious(copyOfPowerPlants, i, removeFromPrevious, powerPlants);

                    if (success)
                    {
                        remaningLoad += removeFromPrevious;
                        powerPlants.Add(currentPowerPlant.Name!, minimumAllowed);
                        remaningLoad -= minimumAllowed;
                    }
                }
            }

            if (remaningLoad != 0)
            {
                return ScenarioResult.AsError("Power plants calculation failed. Could not reach the target load");
            }

            return ScenarioResult.AsSuccess(powerPlants);
        }

        private bool TryToRemoveDifferenceFromPrevious(List<PowerPlant> copyOfPowerPlants, int i, double removeFromPrevious, Dictionary<string, double> powerPlants)
        {
            // There is no previous to subtract from
            if (i == 0)
            {
                return false;
            }

            var previous = copyOfPowerPlants[i - 1];

            var previousValue = powerPlants[previous.Name];

            var result = previous.TryToProduce(previousValue - removeFromPrevious, out double output, out double minimum);

            if (result) 
            {
                powerPlants[previous.Name] = output;
            }

            return result;

        }
    }
}
