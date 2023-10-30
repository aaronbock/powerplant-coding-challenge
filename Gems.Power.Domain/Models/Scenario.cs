namespace Gems.Power.Domain.Models
{
    public class Scenario
    {
        public double Load { get; set; }
        public IReadOnlyCollection<PowerPlant> PowerPlants { get; set; }
    }
}
