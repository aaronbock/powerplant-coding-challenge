using Gems.Power.Domain.Models;
using Gems.Power.Domain.Services;

namespace Gems.Power.Domain.Tests
{
    public class CalculatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Simulate_WithNullScenario_ReturnsError()
        {
            // Arrange
            var sut = new CalculatorService();

            // Act
            var result = sut.Simulate(null!);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void Simulate_WithEmptyLoad_ReturnsError()
        {
            // Arrange
            var sut = new CalculatorService();
            var payload = new Scenario { Load = 0 };

            // Act
            var result = sut.Simulate(payload);

            // Assert
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void Simulate_WithValidScenario_ReturnsCorrectResponse()
        {
            // Arrange
            var sut = new CalculatorService();

            var gas = new Fuel { Name = "Gas", Price = 5, IsWind = false };
            var wind = new Fuel { Name = "Wind", Price = 70, IsWind = true };
            //var kerozine = new Fuel { Name = "Kerozine", Price = 12, IsWind = false };

            var gasPlant = new PowerPlant
            {
                Name = "Gas",
                Efficiency = 0.5,
                OperatesWith = gas,
                Pmax = 100,
                Pmin = 0,
                Type = PowerPlantFuel.GasFired
            };

            var windPlant = new PowerPlant
            {
                Name = "Wind 1",
                Efficiency = 0.3,
                OperatesWith = wind,
                Pmax = 100,
                Pmin = 0,
                Type = PowerPlantFuel.WindTurbine
            };

            var payload = new Scenario
            {
                Load = 100,
                PowerPlants = new List<PowerPlant> { gasPlant, windPlant }
            };

            // Act
            var result = sut.Simulate(payload);

            // Assert
            Assert.That(result.Success, Is.True);

            Assert.That(result.Powerplants.Count, Is.EqualTo(2));
            Assert.That(result.Powerplants.FirstOrDefault(x => x.Key == "Wind 1").Value, Is.EqualTo(21));
            Assert.That(result.Powerplants.FirstOrDefault(x => x.Key == "Gas").Value, Is.EqualTo(79));
        }


        [Test]
        public void Simulate_WithPowerPlantThatNeedsDoesNotReachTheMinimum_DeductsFromPreviousPowerPlant()
        {
            // Arrange
            var sut = new CalculatorService();

            var gas = new Fuel { Name = "Gas", Price = 10, IsWind = false };
            var wind = new Fuel { Name = "Wind", Price = 70, IsWind = true };
            //var kerozine = new Fuel { Name = "Kerozine", Price = 12, IsWind = false };

            var gasPlant1 = new PowerPlant
            {
                Name = "Gas 1",
                Efficiency = 0.5,
                OperatesWith = gas,
                Pmax = 100,
                Pmin = 0,
                Type = PowerPlantFuel.GasFired
            };

            var gasPlant2 = new PowerPlant
            {
                Name = "Gas 2",
                Efficiency = 0.5,
                OperatesWith = gas,
                Pmax = 100,
                Pmin = 50,
                Type = PowerPlantFuel.GasFired
            };

            var payload = new Scenario
            {
                Load = 110,
                PowerPlants = new List<PowerPlant> { gasPlant1, gasPlant2 }
            };

            // Act
            var result = sut.Simulate(payload);

            // Assert
            Assert.That(result.Success, Is.True);

            Assert.That(result.Powerplants.Count, Is.EqualTo(2));
            Assert.That(result.Powerplants.FirstOrDefault(x => x.Key == "Gas 1").Value, Is.EqualTo(60));
            Assert.That(result.Powerplants.FirstOrDefault(x => x.Key == "Gas 2").Value, Is.EqualTo(50));
        }
    }
}