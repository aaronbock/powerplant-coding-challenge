using AutoMapper;
using Gems.Power.Api.Models;
using Gems.Power.Domain.Contracts;
using Gems.Power.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Gems.Power.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductionPlanController : ControllerBase
{
    private readonly ILogger<ProductionPlanController> _logger;
    private readonly IMapper _mapper;
    private readonly ICalculatorService _calculatorService;

    public ProductionPlanController(ILogger<ProductionPlanController> logger, IMapper mapper, ICalculatorService calculatorService)
    {
        _logger = logger;
        _mapper = mapper;
        _calculatorService = calculatorService;
    }

    [HttpPost]
    public ActionResult SimulateLoad(SimulationRequestViewModel simulationRequestViewModel)
    {
        var payload = _mapper.Map<Scenario>(simulationRequestViewModel);

        var (ok, message) = IsValid(payload);

        if (!ok)
        {
            return BadRequest(message);
        }

        var calculationResult = _calculatorService.Simulate(payload);

        if (!calculationResult.Success)
        {
            return BadRequest(calculationResult.ErrorMessage);
        }

        var result = calculationResult.Powerplants!.Select(x => new { Name = x.Key, P = x.Value });

        return Ok(result);
    }

    // todo maybe use some validation framework? fluent validation is a candidate
    private static (bool ok, string? message) IsValid(Scenario payload)
    {
        if (payload == null)
            return (false, "Payload is empty");

        if (payload.PowerPlants.Any(x => x.OperatesWith == null))
        {
            return (false, "Payload needs to contain the following fuels: " +
                "gas(euro/MWh)" +
                "kerosine(euro/MWh)" +
                "co2(euro/ton)" +
                "wind(%)");
        }

        return (true, null);
    }
}
