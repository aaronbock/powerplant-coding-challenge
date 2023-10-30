namespace Gems.Power.Domain.Models
{
    public class PowerPlant
    {
        public string? Name { get; set; }
        public PowerPlantFuel Type { get; set; }
        public double Efficiency { get; set; }
        public long Pmin { get; set; }
        public long Pmax { get; set; }

        public Fuel OperatesWith { get; set; }

        /// <summary>
        /// Returns the cost of megawatt/hour to operate with the given Fuel/price
        /// </summary>
        /// <param name="fuel"></param>
        /// <returns></returns>
        public double CostOfMegawatt
        {
            get
            {
                if (OperatesWith.IsWind)
                {
                    return 0;
                }

                return OperatesWith.Price / Efficiency;
            }
        }

        /// <summary>
        /// Get the total amount we can produce in this plant
        /// </summary>
        public double? AllProductionAvailable
        {
            get
            {
                if (!OperatesWith.IsWind)
                {
                    return null;
                }

                // if the power plant is a wind one
                // we need to use the % of wind to calculate the actual
                // amount of energy generated
                var windFactor = OperatesWith.Price / 100;

                return Math.Round(Pmax * Efficiency * windFactor, 2);
            }
        }

        /// <summary>
        /// Returns the maximum amount we can extract from this plant, given a target 
        /// </summary>
        /// <param name="targetLoad"></param>
        /// <returns></returns>
        public bool TryToProduce(double targetLoad, out double output, out double minimumAllowed)
        {
            var upperLimit = Pmax;
            minimumAllowed = Pmin;

            if (targetLoad > upperLimit)
            {
                output = upperLimit;
                return true;
            }

            // we cannot try to consume less energy from a plant than allowed
            if (targetLoad < minimumAllowed)
            {
                output = 0;
                return false;
            }

            output = targetLoad;

            return true;
        }

    }
}