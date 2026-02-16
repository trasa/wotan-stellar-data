using NetTopologySuite.Geometries;

namespace WotanStellar.Data.Entities;

public class StarSystem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string SystemType { get; set; } = "";
    public string SourceSystemId { get; set; } = "";
    public double GalacticX { get; set; }
    public double GalacticY { get; set; }
    public double GalacticZ { get; set; }
    public Point Position { get; set; } = null!; // NetTopologySuite Point for 3D geometry
    public double? Mass { get; set; }
    public string? StarSpectralTypeSize { get; set; }
    public double DistanceSolLy { get; set; }
    public double? StarRadius { get; set; }
    public double? StarLuminosity { get; set; }

    public ICollection<Star> Stars { get; set; } = [];
}
