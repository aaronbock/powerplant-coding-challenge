namespace Gems.Power.Domain.Models
{
    public class Scenario
    {
        public double Load { get; set; }
        public List<PowerPlant> PowerPlants { get; set; }
    }
}
