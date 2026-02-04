namespace WotanStellar.Data.Entities;

public class Moon
{
    public int Id { get; set; }
    public int PlanetId { get; set; }
    public string MoonName { get; set; } = string.Empty;
    public string MoonType { get; set; } = string.Empty;
    public double OrbitalRadiusKm { get; set; }
    public double OrbitalPeriodDays { get; set; }
    public double MassEarthMasses { get; set; }
    public double RadiusEarthRadii { get; set; }
    public string? SurfaceType { get; set; }
    public int Seed { get; set; }

    public Planet Planet { get; set; } = null!;
}
