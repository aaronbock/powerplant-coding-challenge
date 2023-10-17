namespace Gems.Power.Api.Models
{
    public record PowerplantViewModel(string Name, 
        string Type, 
        double Efficiency, 
        long Pmin, 
        long Pmax);
}
