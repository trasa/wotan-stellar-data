using NetTopologySuite.Geometries;

namespace WotanStellar.Data.Entities;

public class Star
{
    public int Id { get; set; }
    public int StarSystemId { get; set; }
    public string StarName { get; set; } = "";
    public string StarType { get; set; } = "";
    public string SourceSystemId { get; set; } = "";
    public double GalacticX { get; set; }
    public double GalacticY { get; set; }
    public double GalacticZ { get; set; }
    public Point Position { get; set; } = null!;
    public double? StarMass { get; set; }
    public string? StarSpectralTypeSize { get; set; }
    public double DistanceSolLy { get; set; }
    public double? StarRadius { get; set; }
    public double? StarLuminosity { get; set; }

    public StarSystem StarSystem { get; set; } = null!;
    public ICollection<Planet> Planets { get; set; } = [];
}
