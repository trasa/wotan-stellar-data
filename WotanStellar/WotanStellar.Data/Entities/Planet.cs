namespace WotanStellar.Data.Entities;

public class Planet
{
    public int Id { get; set; }
    public int StarId { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlanetType PlanetType { get; set; }
    public double OrbitalRadiusAu { get; set; }
    public double OrbitalPeriodDays { get; set; }
    public double MassEarthMasses { get; set; }
    public double RadiusEarthRadii { get; set; }
    public double? SurfaceTemperatureK { get; set; }
    public string? AtmosphereType { get; set; }
    public string? SurfaceType { get; set; }
    public int Seed { get; set; }

    public Star Star { get; set; } = null!;
    public ICollection<Moon> Moons { get; set; } = [];
    public PlanetaryRingSystem? RingSystem { get; set; }

    public const double EarthRadiusKm = 6378.14;
    public const double EarthMassKg = 5.972e24;

    public double RadiusMeters => RadiusEarthRadii * EarthRadiusKm * 1000;
    public double MassKg => MassEarthMasses * EarthMassKg;

    /// <summary>
    /// Calculate Planet Density in kg/m³ (assuming spherical shape)
    /// </summary>
    public double Density => MassKg / (4.0/3.0 * Math.PI * Math.Pow(RadiusMeters, 3));

    public bool IsGasGiantLike =>  MassKg > 1.898e27 * 0.1; // 10% Jupiter mass
    public bool IsIceGiantLike => MassKg > 8.681e25 * 0.5 && MassKg < 1.898e27 * 0.1;


    /// <summary>
    /// The Roche Limit in meters
    /// </summary>
    /// <remarks>
    /// d ≈ 2.456 * R * (ρ_body / ρ_satellite)^(1/3)
    /// </remarks>
    /// <returns></returns>
    public double RocheLimitMeters
    {
        get
        {
            const double satelliteDensity = 1000.0; // kg/m³, assume icy material
            return 2.456 * RadiusMeters * Math.Pow(Density / satelliteDensity, 1.0 / 3.0);
        }
    }

    /// <summary>
    /// Calculate the Roche Limit in terms of planetary radii (i.e., how many planet radii out the Roche Limit is)
    /// </summary>
    /// <returns></returns>
    public double RocheLimitRadii => RocheLimitMeters / RadiusMeters;

    public double CalculateRingOuterEdge()
    {
        if (Moons.Count == 0)
        {
            return RocheLimitRadii * 4.0;
        }
        // find innermost significant moon (larger than 10km radius)
        var firstSignificantMoon = Moons
            .Where(m => m.RadiusMeters > 10_000.0)
            .OrderBy(m => OrbitalRadiusAu)
            .FirstOrDefault();
        if (firstSignificantMoon == null)
        {
            return RocheLimitRadii * 4.0;
        }
        // Rings should end before the first major moon
        return firstSignificantMoon.OrbitalRadiusKm * 0.8;
    }

    public List<double> CalculateMoonResonances()
    {
        if (Moons.Count == 0)
        {
            return [];
        }

        // Calculate 2:1, 3:1, 3:2 resonances with major moons
        var resonances = new List<double>();
        foreach (var moon in Moons.Where(m => m.RadiusMeters > 10_000.0))
        {
            // 2:1 resonance (particle orbits twice while moon orbits once)
            resonances.Add(moon.OrbitalRadiusKm * Math.Pow(0.5, 2.0 / 3.0));

            // 3:1 resonance
            resonances.Add(moon.OrbitalRadiusKm * Math.Pow(1.0/3.0, 2.0/3.0));

            // 3:2 resonance
            resonances.Add(moon.OrbitalRadiusKm * Math.Pow(2.0/3.0, 2.0/3.0));
        }
        return resonances;
    }

    /// <summary>
    /// Determine if this planet might have rings or not, randomly
    /// Use mass, instead of planet type, to make random determination
    /// </summary>
    /// <param name="random">A random instance seeded with the planet's seed and "rings" for consistency</param>
    /// <returns>true if we decide it should, false otherwise</returns>
    public bool ShouldHaveRings(Random random)
    {
        // calculate based on mass, not planet type
        if (IsGasGiantLike)
        {
            return random.NextDouble() < 0.7;
        }

        if (IsIceGiantLike)
        {
            return random.NextDouble() < 0.5;
        }
        return random.NextDouble() < 0.05;
    }

}

public enum PlanetType
{
    DwarfPlanet,
    FrozenTerrestrial,
    GasGiant,
    HotTerrestrial,
    IceGiant,
    LavaWorld,
    OceanWorld,
    Terrestrial,
}
