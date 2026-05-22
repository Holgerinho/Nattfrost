namespace NattfrostBackend.Models;

/// <summary>
/// Immutable record that stores the result of a geocoding lookup.
/// </summary>
public record LocationCoordinates(string CityName, double Latitude, double Longitude);
