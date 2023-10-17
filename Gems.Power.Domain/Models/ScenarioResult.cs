namespace Gems.Power.Domain.Models
{
    public class ScenarioResult
    {
        public Dictionary<string, double>? Powerplants { get; private set; }
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ScenarioResult AsError(string message)
        {
            return new ScenarioResult { Success = false, ErrorMessage = message };  
        }

        public static ScenarioResult AsSuccess(Dictionary<string, double> powerPlants)
        {
            return new ScenarioResult { Success = true, Powerplants = powerPlants, ErrorMessage = null };
        }

        private ScenarioResult() { }
    }
}
