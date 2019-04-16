using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AlienPathFinder2 {

    IAlienGrid2 grid;

    public AlienPathFinder2(IAlienGrid2 grid) {
        this.grid = grid;
    }

    public Output BestMoveLocation(Vector2 start, int maxMove) {
        var gridIterator = new RadiusGridIterator(grid, start, maxMove);
        var processedSquares = new List<ProcessedSquare>();
        foreach (var square in gridIterator.Squares()) {
            if (grid.CouldMoveThereIfSomeoneGetsOutOfTheWay(square)) {
                var pathFinder = new PathFinder2(grid);
                var result = pathFinder.ShortestPath(start);
                processedSquares.Add(new ProcessedSquare {
                    square = square,
                    facing = Vector2.zero, // TODO
                    distance = result.pathLength
                });
            }
        }
        var bestSquare = processedSquares.OrderByDescending(item => item.distance).Last();
        return new Output {
            targetLocation = bestSquare.square,
            facing = bestSquare.facing
        };
    }

    public struct Output {
        public Vector2 targetLocation;
        public Vector2 facing;
    }
    
    private struct ProcessedSquare {
        public Vector2 square;
        public Vector2 facing;
        public int distance;
    }
}

public class AlienPathingMapWrapper : IAlienGrid2 {

    public AlienPathingMapWrapper(Map map, List<Alien> unmovedAliens) {
        this.map = map;
        this.unmovedAliens = unmovedAliens;
    }

    Map map;
    List<Alien> unmovedAliens;
    
    public bool Pathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile == null || tile.actor != null && tile.GetActor<Alien>() == null) return false;
        return tile.open;
    }
    
    public bool ShouldIterate(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile == null) return false;
        return tile.open;
    }
    
    public bool FinishLocation(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        return DistanceToNearestTarget(gridLocation) == 1 && tile.actor == null;
    }
    
    public int HeuristicFor(Vector2 gridLocation) {
        return DistanceToNearestTarget(gridLocation);
    }
    
    public bool CouldMoveThereIfSomeoneGetsOutOfTheWay(Vector2 calculatedMoveLocation) {
        var tile = map.GetTileAt(calculatedMoveLocation);
        return tile.actor == null || unmovedAliens.Contains(tile.actor.GetComponent<Alien>());
    }
    
    int DistanceToNearestTarget(Vector2 start) {
        return (int)map.GetActors<Soldier>().Select(soldier => Mathf.Abs(soldier.gridLocation.x - start.x) + Mathf.Abs(soldier.gridLocation.y - start.y)).Max();
    }
}

public interface IAlienGrid2 : IIterableGrid, IPathable2 {
    bool CouldMoveThereIfSomeoneGetsOutOfTheWay(Vector2 calculatedMoveLocation);
}
