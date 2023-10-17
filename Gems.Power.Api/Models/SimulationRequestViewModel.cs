namespace Gems.Power.Api.Models
{
    public record SimulationRequestViewModel(long Load, 
        Dictionary<string, double> Fuels, 
        List<PowerplantViewModel> Powerplants);
}
