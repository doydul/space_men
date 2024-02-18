using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {

    enum Facing {
        North,
        South,
        West,
        East
    }

    static Facing[] Facings = { Facing.North, Facing.South, Facing.West, Facing.East };

    class Element {

    }

    class Corridor : Element {
        public int length;
        public Facing direction;
        public Element northWestEnd;
        public Element SouthEastEnd;
    }

    class Room : Element {
        
    }

    class Cursor {
        public int x;
        public int y;

        public Cursor(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void Move(Facing direction) {
            if (direction == Facing.North) {
                y += 1;
            } else if (direction == Facing.South) {
                y -= 1;
            } else if (direction == Facing.East) {
                x += 1;
            } else if (direction == Facing.West) {
                x -= 1;
            }
        }
    }
    
    public MapLayout Generate() {
        var corridors = new List<Corridor>();
        for (int i = 0; i < 5; i++) {
            var corridor = new Corridor {
                length = Random.Range(2, 5),
                direction = Facings.Sample()
            };
            corridors.Add(corridor);
        }
        var walls = new MapLayout();
        var cursor = new Cursor(0, 0);
        walls.AddOpenTile(cursor.x, cursor.y);
        foreach (var corridor in corridors) {
            for (int i = 0; i < corridor.length; i++) {
                cursor.Move(corridor.direction);
                walls.AddOpenTile(cursor.x, cursor.y);
            }
        }
        return walls;
    }
}

public struct MapPoint {
    public int x;
    public int y;
    
    public MapPoint(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public class MapLayout {
    public List<List<bool>> walls => CalculateWalls();

    List<MapPoint> openTiles = new();

    public void AddOpenTile(int x, int y) {
        openTiles.Add(new MapPoint(x, y));
    }

    private List<List<bool>> CalculateWalls() {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach (var point in openTiles) {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }
        var xMod = -minX + 2;
        var yMod = -minY + 2;
        var width = maxX - minX + 5;
        var height = maxY - minY + 5;
        var walls = new List<List<bool>>();
        for (int x = 0; x < width; x++) {
            walls.Add(new List<bool>());
            for (int y = 0; y < height; y++) {
                walls[x].Add(true);
            }
        }
        foreach (var point in openTiles) {
            walls[point.x + xMod][point.y + yMod] = false;
        }
        return walls;
    }
}