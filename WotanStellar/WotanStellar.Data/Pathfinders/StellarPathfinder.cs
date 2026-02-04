using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Pathfinders;

public interface IStellarPathfinder
{

    /// <summary>
    /// Find nearest systems to a given coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<List<StarSystem>> FindNearestStars(double x, double y, double z, int count);

    /// <summary>
    /// Find systems within N light years of a given coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="radiusLy"></param>
    /// <returns></returns>
    Task<List<StarSystem>> FindSystemsWithinRadius(double x, double y, double z, double radiusLy);

    /// <summary>
    /// Get stars within range of a given system (for A* graphing)
    /// </summary>
    /// <param name="fromStar"></param>
    /// <param name="maxJumpRangeLy"></param>
    /// <returns></returns>
    Task<List<StarSystem>> GetReachableStars(StarSystem fromStar, double maxJumpRangeLy);
}

public class StellarPathfinder : IStellarPathfinder
{
    private readonly StellarContext _context;
    private readonly GeometryFactory _geometryFactory;

    public StellarPathfinder(StellarContext context)
    {
        _context = context;
        _geometryFactory = new GeometryFactory(new PrecisionModel(), 0);
    }

    public async Task<List<StarSystem>> FindSystemsWithinRadius(double x, double y, double z, double radiusLy)
    {
        var point = _geometryFactory.CreatePoint(new CoordinateZ(x, y, z));

        return await _context.StarSystems
            .Where(s => s.Position.IsWithinDistance(point, radiusLy))
            .OrderBy(s => s.Position.Distance(point))
            .ToListAsync();
    }

    public async Task<List<StarSystem>> FindNearestStars(double x, double y, double z, int count)
    {
        var point = _geometryFactory.CreatePoint(new CoordinateZ(x, y, z));

        return await _context.StarSystems
            .OrderBy(s => s.Position.Distance(point))
            .Take(count)
            .ToListAsync();
    }

   public async Task<List<StarSystem>> GetReachableStars(StarSystem fromStar, double maxJumpRangeLy)
    {
        return await _context.StarSystems
            .Where(s => s.Id != fromStar.Id && s.Position.IsWithinDistance(fromStar.Position, maxJumpRangeLy))
            .ToListAsync();
    }
}
