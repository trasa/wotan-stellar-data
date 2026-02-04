using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Pathfinders;

public interface IRoutePathfinder
{
    Task<List<StarSystem>> FindPath(StarSystem start, StarSystem goal, double maxJumpRangeLy);
}

public class AStarPathfinder : IRoutePathfinder
{
    private readonly StellarPathfinder _stellarPathfinder;

    public AStarPathfinder(StellarPathfinder stellarPathfinder)
    {
        _stellarPathfinder = stellarPathfinder;
    }

    public async Task<List<StarSystem>> FindPath(
        StarSystem start,
        StarSystem goal,
        double maxJumpRangeLy)
    {
        var openSet = new PriorityQueue<StarSystem, double>();
        var cameFrom = new Dictionary<int, StarSystem>();
        var gScore = new Dictionary<int, double>();
        var fScore = new Dictionary<int, double>();

        gScore[start.Id] = 0;
        fScore[start.Id] = HeuristicDistance(start, goal);
        openSet.Enqueue(start, fScore[start.Id]);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current.Id == goal.Id)
            {
                return ReconstructPath(cameFrom, current);
            }

            var neighbors = await _stellarPathfinder.GetReachableStars(current, maxJumpRangeLy);

            foreach (var neighbor in neighbors)
            {
                var tentativeGScore = gScore[current.Id] + Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor.Id) || tentativeGScore < gScore[neighbor.Id])
                {
                    cameFrom[neighbor.Id] = current;
                    gScore[neighbor.Id] = tentativeGScore;
                    fScore[neighbor.Id] = tentativeGScore + HeuristicDistance(neighbor, goal);

                    openSet.Enqueue(neighbor, fScore[neighbor.Id]);
                }
            }
        }

        return new List<StarSystem>(); // No path found
    }

    private double Distance(StarSystem a, StarSystem b)
    {
        return a.Position.Distance(b.Position);
    }

    private double HeuristicDistance(StarSystem a, StarSystem b)
    {
        // Euclidean distance as heuristic
        return a.Position.Distance(b.Position);
    }

    private List<StarSystem> ReconstructPath(Dictionary<int, StarSystem> cameFrom, StarSystem current)
    {
        var path = new List<StarSystem> { current };

        while (cameFrom.ContainsKey(current.Id))
        {
            current = cameFrom[current.Id];
            path.Insert(0, current);
        }

        return path;
    }
}
