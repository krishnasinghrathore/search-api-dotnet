using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly List<Location> _locations = new List<Location>
    {
        new Location { Id = 1, Name = "supermarket", OpeningTime = DateTime.Today.AddHours(8), ClosingTime = DateTime.Today.AddHours(18) },
        new Location { Id = 2, Name = "restaurant", OpeningTime = DateTime.Today.AddHours(9), ClosingTime = DateTime.Today.AddHours(17) },
        // Add more locations as needed
    };

    [HttpGet]
    public IActionResult GetAvailableLocations()
    {
        var currentTime = DateTime.Now;
        var availableLocations = _locations
            .Where(loc => currentTime >= loc.OpeningTime && currentTime <= loc.ClosingTime)
            .Select(loc => new { loc.Id, loc.Name })
            .ToList();

        return Ok(availableLocations);
    }
    [HttpPost("addlocation")]
    public IActionResult AddLocation([FromBody] Location newLocation)
    {
        if (newLocation == null)
        {
            return BadRequest("Invalid location data");
        }

        // Set default opening and closing times if not provided
        newLocation.OpeningTime = newLocation.OpeningTime == default ? DateTime.Today.AddHours(8) : newLocation.OpeningTime;
        newLocation.ClosingTime = newLocation.ClosingTime == default ? DateTime.Today.AddHours(18) : newLocation.ClosingTime;

        newLocation.Id = _locations.Count + 1; // Replace with your actual logic for generating IDs
        _locations.Add(newLocation);

        return CreatedAtAction(nameof(GetAvailableLocations), new { id = newLocation.Id }, newLocation);
    }
}
