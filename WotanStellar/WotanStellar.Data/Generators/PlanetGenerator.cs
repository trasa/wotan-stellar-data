using Microsoft.Extensions.Logging;
using WotanStellar.Data.Entities;
using WotanStellar.Data.Formatters;

namespace WotanStellar.Data.Generators;

public interface IPlanetGenerator
{
    void GeneratePlanets(StarSystem system);
}

public class PlanetGenerator : IPlanetGenerator
{
    private readonly ILogger<PlanetGenerator> _logger;
    private readonly IMoonGenerator _moonGenerator;

    public PlanetGenerator(ILogger<PlanetGenerator> logger, IMoonGenerator moonGenerator)
    {
        _logger = logger;
        _moonGenerator = moonGenerator;
    }

    public void GeneratePlanets(StarSystem system)
    {
        // Use star system ID as base seed for deterministic generation
        var random = new Random(system.Id);

        _logger.LogWarning("Generating planets for system {SystemId}, has {Count} stars", system.Id, system.Stars.Count);
        foreach (var star in system.Stars)
        {
            GeneratePlanets(star, random);
        }
    }

    private void GeneratePlanets(Star star, Random random)
    {
        _logger.LogWarning("Generating planets for star {StarName} (ID: {StarId})", star.StarName, star.Id);
        var planetCount = DeterminePlanetCount(star, random);
        _logger.LogInformation("Generating {PlanetCount} planets for star {StarName}", planetCount, star.StarName);
        for (var i = 0; i < planetCount; i++)
        {
            var planet = GeneratePlanet(star, i, random);
            star.Planets.Add(planet);
            _moonGenerator.GenerateMoons(planet, random);
        }
    }

    private int DeterminePlanetCount(Star star, Random random)
    {
        // Stars with higher mass tend to have more planets
        // G-type stars (like our Sun): 4-8 planets
        // M-type stars (red dwarfs): 2-6 planets
        // K-type stars: 3-7 planets

        if (star.StarMass == null)
            return random.Next(2, 6);

        var mass = star.StarMass.Value;

        return mass switch
        {
            // Heavier stars
            > 1.0 => random.Next(3, 9),
            // Sun-like
            > 0.5 => random.Next(4, 8),
            _ => random.Next(2, 6) // red dwarfs
        };
    }

    private Planet GeneratePlanet(Star star, int index, Random random)
    {
        // Orbital radius increases with each planet (roughly exponential)
        var orbitalRadiusAu = 0.4 * Math.Pow(1.7, index) * (0.8 + random.NextDouble() * 0.4);

        // Calculate temperature based on distance from star
        var starLuminosity = star.StarLuminosity ?? 1.0;
        var effectiveTemp = CalculateEffectiveTemperature(orbitalRadiusAu, starLuminosity);

        // Determine planet type based on distance and temperature
        string planetType = DeterminePlanetType(orbitalRadiusAu, effectiveTemp, random);

        // Generate physical properties based on type
        var (mass, radius) = GeneratePlanetPhysics(planetType, random);

        // Calculate orbital period using Kepler's third law
        var starMass = star.StarMass ?? 1.0;
        var orbitalPeriodDays = CalculateOrbitalPeriod(orbitalRadiusAu, starMass);

        _logger.LogInformation("Generated planet {PlanetName} around star {StarName}: Type={PlanetType}, OrbitalRadius={OrbitalRadiusAu} AU, OrbitalPeriod={OrbitalPeriodDays} days, Mass={MassEarthMasses} Earth masses, Radius={RadiusEarthRadii} Earth radii, Temp={SurfaceTemperatureK} K",
            string.Format(RomanNumeralFormatter.Instance, "{0} {1:R}", star.StarName, index + 1),
            star.StarName,
            planetType,
            orbitalRadiusAu,
            orbitalPeriodDays,
            mass,
            radius,
            effectiveTemp);
        return new Planet
        {
            StarId = star.Id,
            PlanetName = string.Format(RomanNumeralFormatter.Instance, "{0} {1:R}", star.StarName, index + 1),
            PlanetType = planetType,
            OrbitalRadiusAu = orbitalRadiusAu,
            OrbitalPeriodDays = orbitalPeriodDays,
            MassEarthMasses = mass,
            RadiusEarthRadii = radius,
            SurfaceTemperatureK = effectiveTemp,
            AtmosphereType = DetermineAtmosphereType(planetType, mass, effectiveTemp, random),
            SurfaceType = DetermineSurfaceType(planetType, effectiveTemp, random),
            Seed = HashCode.Combine(star.Id, index)
        };
    }

    private double CalculateEffectiveTemperature(double orbitalRadiusAu, double starLuminosity)
    {
        // Stefan-Boltzmann law simplified for planet temperature
        // T = 278.5 K * (L/d²)^0.25
        // Where L is star luminosity (solar units), d is distance in AU
        return 278.5 * Math.Pow(starLuminosity / (orbitalRadiusAu * orbitalRadiusAu), 0.25);
    }

    private string? DetermineSurfaceType(string planetType, double temp, Random random)
    {
        return planetType switch
        {
            "lava_world" => "molten",
            "hot_terrestrial" => temp > 500 ? "scorched_rock" : "rocky",
            "terrestrial" => random.NextDouble() < 0.6 ? "rocky" : "varied_terrain",
            "ocean_world" => "water_ice",
            "frozen_terrestrial" => "icy_rock",
            "gas_giant" or "ice_giant" => "gaseous",
            "dwarf_planet" => "icy",
            _ => "rocky"
        };
    }

    private double CalculateOrbitalPeriod(double orbitalRadiusAu, double starMass)
    {
        // Kepler's third law: P² = a³ / M
        // P in years, a in AU, M in solar masses
        double periodYears = Math.Sqrt(Math.Pow(orbitalRadiusAu, 3) / starMass);
        return periodYears * 365.25; // Convert to days
    }

    private (double mass, double radius) GeneratePlanetPhysics(string planetType, Random random)
    {
        return planetType switch
        {
            "lava_world" => (random.NextDouble() * 2 + 0.5, random.NextDouble() * 0.5 + 0.8),
            "hot_terrestrial" => (random.NextDouble() * 1.5 + 0.3, random.NextDouble() * 0.4 + 0.7),
            "terrestrial" => (random.NextDouble() * 2 + 0.4, random.NextDouble() * 0.6 + 0.5),
            "ocean_world" => (random.NextDouble() * 3 + 0.8, random.NextDouble() * 0.8 + 0.9),
            "frozen_terrestrial" => (random.NextDouble() * 1.5 + 0.3, random.NextDouble() * 0.5 + 0.6),
            "gas_giant" => (random.NextDouble() * 300 + 50, random.NextDouble() * 8 + 6),
            "ice_giant" => (random.NextDouble() * 20 + 10, random.NextDouble() * 3 + 3),
            "dwarf_planet" => (random.NextDouble() * 0.01 + 0.001, random.NextDouble() * 0.2 + 0.1),
            _ => (1.0, 1.0)
        };
    }

    private string? DetermineAtmosphereType(string planetType, double mass, double temp, Random random)
    {
        if (planetType.Contains("gas") || planetType.Contains("ice_giant"))
            return "thick_hydrogen_helium";

        if (mass < 0.1 || temp > 400) // Too small or too hot to hold atmosphere
            return "none";

        if (temp < 150)
            return random.NextDouble() < 0.3 ? "thin_nitrogen" : "none";

        var roll = random.NextDouble();
        return roll switch
        {
            < 0.3 => "nitrogen_oxygen",
            < 0.5 => "carbon_dioxide",
            < 0.7 => "thick_carbon_dioxide",
            < 0.9 => "thin_nitrogen",
            _ => "toxic"
        };
    }

    private string DeterminePlanetType(double orbitalRadiusAu, double temp, Random random)
    {
        // Frost line is typically around 2.7 AU for a Sun-like star
        const double frostLine = 2.7;

        switch (orbitalRadiusAu)
        {
            case < 0.5:
                return random.NextDouble() < 0.7 ? "hot_terrestrial" : "lava_world";
            case < frostLine:
                return random.NextDouble() < 0.8 ? "terrestrial" : "ocean_world";
            case < frostLine * 2:
            {
                var roll = random.NextDouble();
                return roll switch
                {
                    < 0.5 => "gas_giant",
                    < 0.8 => "ice_giant",
                    _ => "frozen_terrestrial"
                };
            }
            default:
            {
                var roll = random.NextDouble();
                return roll switch
                {
                    < 0.4 => "gas_giant",
                    < 0.7 => "ice_giant",
                    _ => "dwarf_planet"
                };
            }
        }
    }
}
