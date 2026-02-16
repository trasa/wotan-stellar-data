namespace WotanStellar.Data.Entities;

public class PlanetaryRingSystem
{
    public PlanetaryRingSystem() {}

    public PlanetaryRingSystem(Planet planet)
    {
        Id = planet.Id;
        Planet = planet;
    }

    public int Id { get; set; } // same as Planet.Id
    public double InnerEdgeRadius { get; set; } // in planetary radii
    public double OuterEdgeRadius { get; set; } // in planetary radii
    public RingComposition PrimaryComposition { get; set; }
    public double TotalMassKg { get; set; } // in kg
    public int Seed { get; set; }

    public Planet Planet { get; set; } = null!;
    public ICollection<PlanetaryRing> Rings { get; set; } = [];

    public static RingComposition DetermineComposition(Random random, Planet planet)
    {
        return planet.PlanetType switch
        {
            PlanetType.GasGiant => (RingComposition)random.Next(0, 4), // All types possible
            PlanetType.IceGiant => (RingComposition)random.Next(0, 3), // No Dust rings
            _ => RingComposition.Rock // Terrestrial planets likely have rocky rings if any
        };
    }
}


public enum RingComposition
{
    Ice,
    Rock,
    Mixed,
    Dust
}
