using System.Drawing;
using Microsoft.Extensions.Logging;
using WotanStellar.Data.Entities;
using WotanStellar.Data.Extensions;

namespace WotanStellar.Data.Generators;

public interface IPlanetaryRingGenerator
{
    PlanetaryRingSystem? Generate(Planet planet);
}

public class PlanetaryRingGenerator : IPlanetaryRingGenerator
{
    private readonly ILogger<PlanetaryRingGenerator> _logger;

    public PlanetaryRingGenerator(ILogger<PlanetaryRingGenerator> logger)
    {
        _logger = logger;
    }

    public PlanetaryRingSystem? Generate(Planet planet)
    {
        var random = new Random(HashCode.Combine(planet.Seed, "rings"));
        if (!planet.ShouldHaveRings(random))
        {
            // nope
            _logger.LogInformation("Planet {PlanetName} does not have rings.", planet.Name);
            return null;
        }

        var ringSystem = new PlanetaryRingSystem(planet)
        {
            PrimaryComposition = PlanetaryRingSystem.DetermineComposition(random, planet)
        };

        var rocheLimitRadii = planet.RocheLimitRadii;
        ringSystem.InnerEdgeRadius = rocheLimitRadii * random.NextDouble(0.9, 1.1);

        // ensure outer edge doesn't conflict with moons
        var maxRingRadius = planet.CalculateRingOuterEdge();
        ringSystem.OuterEdgeRadius = Math.Min(rocheLimitRadii * random.NextDouble(2.0, 4.0), maxRingRadius);

        _logger.LogDebug("Generating ring system for planet {PlanetName}: Roche Limit = {RocheLimit:F2} radii, Inner Edge = {InnerEdge:F2} radii, Outer Edge = {OuterEdge:F2} radii, Composition = {Composition}",
            planet.Name, rocheLimitRadii, ringSystem.InnerEdgeRadius, ringSystem.OuterEdgeRadius, ringSystem.PrimaryComposition);
        // let's build some rings for this system
        GenerateRings(random, planet, ringSystem);

        ringSystem.TotalMassKg = CalculateRingMass(random, ringSystem);
        _logger.LogInformation("Generated ring system for planet {PlanetName} with {RingCount} rings and total mass {TotalMass:E2} kg.",
            planet.Name, ringSystem.Rings.Count, ringSystem.TotalMassKg);
        return ringSystem;
    }

    private void GenerateRings(Random random, Planet planet, PlanetaryRingSystem ringSystem)
    {
        var numRings = random.Next(1, 12);
        var currentRadius = ringSystem.InnerEdgeRadius;
        var radiusStep = (ringSystem.OuterEdgeRadius - ringSystem.InnerEdgeRadius) / numRings;
        var resonances = planet.CalculateMoonResonances();
        for (var i = 0; i < numRings; i++)
        {
            var ringCenter = currentRadius * radiusStep / 0.5;
            // Check if this location is near a moon resonance
            var nearResonance = resonances.Any(r => Math.Abs(r - ringCenter) < 0.3);
            var ringType = ChooseRingType(random, i, numRings, nearResonance);
            _logger.LogDebug("Ring {RingIndex}: Center = {RingCenter:F2} radii, Near Resonance = {NearResonance}, Chosen Type = {RingType}",
                i, ringCenter, nearResonance, ringType);
            if (ringType == RingType.Gap)
            {
                currentRadius += radiusStep * random.NextDouble(0.3, 0.7);
                continue;
            }

            var ring = new PlanetaryRing
            {
                RingIndex = ringSystem.Rings.Count,
                InnerRadius = currentRadius,
                OuterRadius = currentRadius + radiusStep * random.NextDouble(0.8, 1.2),
                RingType = ringType,
                Density = GenerateDensity(random, ringType),
                Brightness = GenerateBrightness(random, ringSystem.PrimaryComposition),
                RingColor = GenerateColor(random, ringSystem.PrimaryComposition),
            };
            _logger.LogDebug("Created ring {RingIndex} for planet {PlanetName}: Inner Radius = {InnerRadius:F2} radii, Outer Radius = {OuterRadius:F2} radii, Type = {RingType}, Density = {Density:F2}, Brightness = {Brightness:F2}",
                ring.RingIndex, planet.Name, ring.InnerRadius, ring.OuterRadius, ring.RingType, ring.Density, ring.Brightness);
            ringSystem.Rings.Add(ring);
            currentRadius = ring.OuterRadius;
        }
    }

    private static RingType ChooseRingType(Random random, int ringIndex, int totalRings, bool nearResonance)
    {
        var roll = random.NextDouble();
        // force gaps at resonances
        if (nearResonance && roll < 0.7)
        {
            return RingType.Gap;
        }

        // More likely to have gaps between major ring groups
        if (ringIndex > 0 && roll < 0.2)
        {
            return RingType.Gap;
        }

        return roll switch
        {
            < 0.5 => RingType.Dense,
            < 0.85 => RingType.Diffuse,
            _ => RingType.Shepherd
        };
    }

    private static double GenerateDensity(Random random, RingType ringType)
    {
        return ringType switch
        {
            RingType.Dense => random.NextDouble(0.6, 0.95),
            RingType.Diffuse => random.NextDouble(0.15, 0.45),
            RingType.Shepherd => random.NextDouble(0.7, 1.0),
            _ => 0.0
        };
    }

    private static double GenerateBrightness(Random random, RingComposition composition)
    {
        return composition switch
        {
            RingComposition.Ice => random.NextDouble(0.7, 1.0),
            RingComposition.Rock => random.NextDouble(0.2, 0.4),
            RingComposition.Mixed => random.NextDouble(0.4, 0.7),
            RingComposition.Dust => random.NextDouble(0.1, 0.3),
            _ => 0.5
        };
    }

    private static Color GenerateColor(Random random, RingComposition composition)
    {
        return composition switch
        {
            RingComposition.Ice => Color.FromArgb(
                random.Next(230, 255),
                random.Next(240, 255),
                random.Next(245, 255)),
            RingComposition.Rock => Color.FromArgb(
                random.Next(160, 200),
                random.Next(120, 160),
                random.Next(100, 140)),
            RingComposition.Mixed => Color.FromArgb(
                random.Next(200, 240),
                random.Next(190, 230),
                random.Next(180, 220)),
            RingComposition.Dust => Color.FromArgb(
                random.Next(140, 180),
                random.Next(130, 170),
                random.Next(120, 160)),
            _ => Color.White
        };
    }

    private double CalculateRingMass(Random random, PlanetaryRingSystem system)
    {
        // Estimate total ring mass based on ring area and density
        double totalMass = 0;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var ring in system.Rings)
        {
            if (ring.RingType == RingType.Gap)
                continue;

            var innerRadiusMeters = ring.InnerRadius;
            var outerRadiusMeters = ring.OuterRadius;
            var area = Math.PI * (outerRadiusMeters * outerRadiusMeters - innerRadiusMeters * innerRadiusMeters);

            // Surface density varies by ring type and composition
            var surfaceDensity = ring.Density * GetBaseSurfaceDensity(random, system.PrimaryComposition);

            totalMass += area * surfaceDensity;
        }

        return totalMass;
    }

    private double GetBaseSurfaceDensity(Random random, RingComposition composition)
    {
        // kg/m² - Saturn's rings are ~5-50 kg/m²
        return composition switch
        {
            RingComposition.Ice => random.NextDouble(10, 50),
            RingComposition.Rock => random.NextDouble(20, 80),
            RingComposition.Mixed => random.NextDouble(15, 60),
            RingComposition.Dust => random.NextDouble(1, 10),
            _ => 20
        };
    }
}
