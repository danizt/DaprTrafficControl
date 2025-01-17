﻿namespace FineCollectionService.Controllers;

[ApiController]
[Route("")]
public class CollectionController : ControllerBase
{
    private static string _fineCalculatorLicenseKey;
    private readonly ILogger<CollectionController> _logger;
    private readonly IFineCalculator _fineCalculator;
    private readonly VehicleRegistrationService _vehicleRegistrationService;

    public CollectionController(ILogger<CollectionController> logger,
        IFineCalculator fineCalculator, VehicleRegistrationService vehicleRegistrationService)
    {
        _logger = logger;
        _fineCalculator = fineCalculator;
        _vehicleRegistrationService = vehicleRegistrationService;

        // set finecalculator component license-key
        if (_fineCalculatorLicenseKey == null)
        {
            bool runningInACA = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CONTAINER_APP_HOSTNAME"));

            if (runningInACA)
            {
                _fineCalculatorLicenseKey = Environment.GetEnvironmentVariable("finecalculator.licensekey");
            }
            else 
            {
                _fineCalculatorLicenseKey = "HX783-K2L7V-CRJ4A-5PN1G";
            }
        }
    }

    [Route("collectfine")]
    [HttpPost()]
    public async Task<ActionResult> CollectFine(SpeedingViolation speedingViolation)
    {
        decimal fine = _fineCalculator.CalculateFine(_fineCalculatorLicenseKey, speedingViolation.ViolationInKmh);

        // get owner info
        var vehicleInfo = await _vehicleRegistrationService.GetVehicleInfo(speedingViolation.VehicleId);

        // log fine
        string fineString = fine == 0 ? "tbd by the prosecutor" : $"{fine} Euro";
        _logger.LogInformation($"Sent speeding ticket to {vehicleInfo.OwnerName}. " +
            $"Road: {speedingViolation.RoadId}, Licensenumber: {speedingViolation.VehicleId}, " +
            $"Vehicle: {vehicleInfo.Brand} {vehicleInfo.Model}, " +
            $"Violation: {speedingViolation.ViolationInKmh} Km/h, Fine: {fineString}, " +
            $"On: {speedingViolation.Timestamp.ToString("dd-MM-yyyy")} " +
            $"at {speedingViolation.Timestamp.ToString("hh:mm:ss")}.");

        // send fine by email
        // TODO

        return Ok();
    }
}