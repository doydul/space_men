using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AlienPathFinder2 {

    IAlienGrid2 grid;
    BasicAlienPathingWrapper basicGrid;

    public AlienPathFinder2(IAlienGrid2 grid, BasicAlienPathingWrapper basicGrid) {
        this.grid = grid;
        this.basicGrid = basicGrid;
    }

    public Output BestMoveLocation(Vector2 start, int maxMove) {
        // Debug.Log("pathing start ---------------");
        var gridIterator = new RadiusGridIterator(grid, start, maxMove);
        var processedSquares = new List<ProcessedSquare>();
        foreach (var square in gridIterator.Squares()) {
            if (grid.CouldMoveThereIfSomeoneGetsOutOfTheWay(square) || square == start) {
                var pathFinder = new PathFinder2(grid);
                var result = pathFinder.ShortestPath(square);
                // Debug.Log("assessing square");
                // Debug.Log(square);
                // Debug.Log(result.pathLength);

                processedSquares.Add(new ProcessedSquare {
                    square = square,
                    distance = result.pathLength
                });
            }
        }
        var bestSquare = processedSquares.OrderByDescending(item => item.distance).Last();
        bestSquare = processedSquares.Where(item => item.distance == bestSquare.distance)
                         .OrderByDescending(item => Mathf.Abs(start.x - item.square.x) + Mathf.Abs(start.y - item.square.y))
                         .Last();
        basicGrid.target = bestSquare.square;
        var actualPath = new PathFinder2(basicGrid).ShortestPath(start);
        Vector2 facing;
        if (actualPath.previousNode == null) {
            facing = Vector2.zero;
        } else {
            facing = actualPath.square - actualPath.previousNode.square;
        }
        // Debug.Log("best square");
        // Debug.Log(bestSquare.square);
        // Debug.Log(bestSquare.distance);

        return new Output {
            targetLocation = bestSquare.square,
            facing = facing
        };
    }

    public struct Output {
        public Vector2 targetLocation;
        public Vector2 facing;
    }

    private struct ProcessedSquare {
        public Vector2 square;
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
        return DistanceToNearestTarget(gridLocation) == 1;
    }

    public bool BlockedLocation(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        return tile.actor != null;
    }

    public int HeuristicFor(Vector2 gridLocation) {
        return DistanceToNearestTarget(gridLocation);
    }

    public bool CouldMoveThereIfSomeoneGetsOutOfTheWay(Vector2 calculatedMoveLocation) {
        var tile = map.GetTileAt(calculatedMoveLocation);
        return tile.actor == null || unmovedAliens.Contains(tile.actor.GetComponent<Alien>());
    }

    int DistanceToNearestTarget(Vector2 start) {
        return (int)map.GetActors<Soldier>().Select(soldier => Mathf.Abs(soldier.gridLocation.x - start.x) + Mathf.Abs(soldier.gridLocation.y - start.y)).Min();
    }
}

public interface IAlienGrid2 : IIterableGrid, IPathable2 {
    bool CouldMoveThereIfSomeoneGetsOutOfTheWay(Vector2 calculatedMoveLocation);
}

public class BasicAlienPathingWrapper : IPathable2 {

    public BasicAlienPathingWrapper(Map map) {
        this.map = map;
    }

    Map map;
    public Vector2 target;

    public bool Pathable(Vector2 gridLocation) {
        var tile = map.GetTileAt(gridLocation);
        if (tile == null || tile.actor != null && tile.GetActor<Alien>() == null) return false;
        return tile.open;
    }

    public bool FinishLocation(Vector2 gridLocation) {
        return gridLocation == target;
    }

    public bool BlockedLocation(Vector2 gridLocation) {
        return false;
    }

    public int HeuristicFor(Vector2 gridLocation) {
        return (int)(Mathf.Abs(gridLocation.x - target.x) + Mathf.Abs(gridLocation.y - target.y));
    }
}
