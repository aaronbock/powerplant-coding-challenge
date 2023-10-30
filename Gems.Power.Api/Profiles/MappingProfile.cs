using AutoMapper;
using Gems.Power.Api.Models;
using Gems.Power.Domain.Models;

namespace Gems.Power.Api.Profiles
{
    public class MappingProfile : Profile
    {
        // todo maybe move to the domain class?
        const string GAS = "gas(euro/MWh)";
        const string KEROSINE = "kerosine(euro/MWh)";
        const string WIND = "wind(%)";

        public MappingProfile()
        {
            CreateMap<SimulationRequestViewModel, Scenario>()
                .AfterMap(MapFuel);
            CreateMap<PowerplantViewModel, PowerPlant>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => ConvertType(y)));
        }

        private static void MapFuel(SimulationRequestViewModel src, Scenario dest)
        {
            if (src?.Fuels == null)
                return;

            var gas = GetFuel(src, GAS, false);
            var kerosine = GetFuel(src, KEROSINE, false);
            var wind = GetFuel(src, WIND, true);

            dest.PowerPlants.ToList().ForEach(p => 
            {
                switch (p.Type)
                {
                    case PowerPlantFuel.GasFired:
                        p.OperatesWith = gas;
                        break;
                    case PowerPlantFuel.Turbojet:
                        p.OperatesWith = kerosine;
                        break;
                    case PowerPlantFuel.WindTurbine:
                        p.OperatesWith = wind;
                        break;
                    default:
                        break;
                }
            });
        }

        private static Fuel GetFuel(SimulationRequestViewModel src, string fuelName, bool isWind)
        {
            var fuelDictionary = src.Fuels.FirstOrDefault(x => x.Key == fuelName);
            return new Fuel { Name = fuelDictionary.Key, Price = fuelDictionary.Value, IsWind = isWind };
        }

        private static PowerPlantFuel ConvertType(PowerplantViewModel model)
        {
            return model.Type switch
            {
                "gasfired" => PowerPlantFuel.GasFired,
                "turbojet" => PowerPlantFuel.Turbojet,
                "windturbine" => PowerPlantFuel.WindTurbine,
                _ => PowerPlantFuel.Unknown,
            };
        }
    }
}
