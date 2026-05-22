using Microsoft.AspNetCore.Mvc;
using NattfrostBackend.Services;

namespace NattfrostBackend.Controllers;

/// <summary>
/// Endpoints for manual testing and debugging of weather/frost functionality.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly OpenMeteoService _openMeteo;
    private readonly FrostCheckBackgroundService _backgroundService;

    public WeatherController(OpenMeteoService openMeteo, FrostCheckBackgroundService backgroundService)
    {
        _openMeteo = openMeteo;
        _backgroundService = backgroundService;
    }

    /// <summary>
    /// Checks a single city for frost risk in the next 16 hours.
    /// GET /api/weather/frostrisk?city=Gothenburg
    /// </summary>
    [HttpGet("frostrisk")]
    public async Task<IActionResult> GetFrostRisk([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest(new { error = "City name is required." });

        try
        {
            var coords = await _openMeteo.GetCoordinatesAsync(city);
            var hasFrostRisk = await _openMeteo.HasFrostRiskAsync(coords.Latitude, coords.Longitude);

            return Ok(new
            {
                city = coords.CityName,
                latitude = coords.Latitude,
                longitude = coords.Longitude,
                hasFrostRisk
            });
        }
        catch (GeocodingException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ForecastException ex)
        {
            return StatusCode(502, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Manually triggers the full subscriber frost check immediately.
    /// GET /api/weather/check
    /// </summary>
    [HttpGet("check")]
    public async Task<IActionResult> TriggerFrostCheck()
    {
        await _backgroundService.RunFrostCheckAsync(HttpContext.RequestAborted);

        return Ok(new { message = "Frost check completed. Check logs for results." });
    }
}
