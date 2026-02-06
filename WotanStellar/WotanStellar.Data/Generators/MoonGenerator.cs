using Microsoft.Extensions.Logging;
using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Generators;

public interface IMoonGenerator
{
    void GenerateMoons(Planet planet, Random random);
}
public class MoonGenerator : IMoonGenerator
{
    private readonly ILogger<MoonGenerator> _logger;

    public MoonGenerator(ILogger<MoonGenerator> logger)
    {
        _logger = logger;
    }

    public void GenerateMoons(Planet planet, Random random)
    {
        _logger.LogInformation("Generating moons for planet {PlanetName} (ID: {PlanetId})", planet.PlanetName, planet.Id);
        int moonCount = DetermineMoonCount(planet, random);
        _logger.LogInformation("Generating {MoonCount} moons for planet {PlanetName}", moonCount, planet.PlanetName);
        for (int i = 0; i < moonCount; i++)
        {
            var moon = GenerateMoon(planet, i, random);
            planet.Moons.Add(moon);
        }
    }

    private int DetermineMoonCount(Planet planet, Random random)
    {
        // Gas giants get more moons
        if (planet.PlanetType.Contains("gas_giant"))
            return random.Next(5, 20);

        if (planet.PlanetType.Contains("ice_giant"))
            return random.Next(3, 15);

        // Terrestrial planets get fewer moons
        if (planet.MassEarthMasses > 0.5)
            return random.Next(0, 3);

        return random.Next(0, 2);
    }

    private Moon GenerateMoon(Planet planet, int index, Random random)
    {
        // Moon orbital radius (in km) - closer moons for smaller planets
        double baseRadius = 50000 * Math.Sqrt(planet.MassEarthMasses);
        double orbitalRadiusKm = baseRadius * Math.Pow(2, index) * (0.8 + random.NextDouble() * 0.4);

        // Moon mass (typically much smaller than planet)
        double moonMass = planet.MassEarthMasses * random.NextDouble() * 0.01;
        double moonRadius = Math.Pow(moonMass, 0.27); // Mass-radius relationship

        // Calculate orbital period (simplified)
        double orbitalPeriodDays = Math.Sqrt(Math.Pow(orbitalRadiusKm / 384400, 3)) * 27.3; // Scaled to Earth's Moon

        _logger.LogInformation("Generated moon {MoonName} for planet {PlanetName}: Mass={MoonMass} Earth masses, Radius={MoonRadius} Earth radii, OrbitalRadius={OrbitalRadiusKm} km, OrbitalPeriod={OrbitalPeriodDays} days",
            $"{planet.PlanetName} {(char)('a' + index)}", planet.PlanetName, moonMass, moonRadius, orbitalRadiusKm, orbitalPeriodDays);

        return new Moon
        {
            PlanetId = planet.Id,
            MoonName = $"{planet.PlanetName} {(char)('a' + index)}",
            MoonType = DetermineMoonType(moonMass, planet.SurfaceTemperatureK ?? 200, random),
            OrbitalRadiusKm = orbitalRadiusKm,
            OrbitalPeriodDays = orbitalPeriodDays,
            MassEarthMasses = moonMass,
            RadiusEarthRadii = moonRadius,
            SurfaceType = DetermineMoonSurfaceType(moonMass, random),
            Seed = HashCode.Combine(planet.Seed, index)
        };
    }

    private string DetermineMoonType(double mass, double temp, Random random)
    {
        if (mass < 0.001) return "asteroid_moon";
        if (temp < 150) return "ice_moon";
        return mass > 0.01 ? "large_moon" : "rocky_moon";
    }

    private string DetermineMoonSurfaceType(double mass, Random random)
    {
        if (mass < 0.001) return "cratered";

        var roll = random.NextDouble();
        return roll switch
        {
            < 0.4 => "icy",
            < 0.7 => "rocky",
            _ => "mixed_ice_rock"
        };
    }
}
