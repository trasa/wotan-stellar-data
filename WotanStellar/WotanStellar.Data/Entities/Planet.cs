namespace WotanStellar.Data.Entities;

public class Planet
{
    public int Id { get; set; }
    public int StarId { get; set; }
    public string PlanetName { get; set; } = string.Empty;
    public string PlanetType { get; set; } = string.Empty;
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

}
