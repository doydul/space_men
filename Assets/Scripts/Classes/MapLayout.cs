using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapLayout {

    public class Tile {
        public MapPoint point;
        public int roomId;
        public bool isWall;
        public bool isAlienSpawner;
        public bool isPlayerSpawner;
        public bool isLootSpawner;
        public bool isSpecial => isAlienSpawner || isPlayerSpawner || isLootSpawner;
        public bool ignoreOverlap;
        public Door.Facing doorFacing;
        public bool behindDoor;

        public override string ToString() => $"Tile(point: {point}, isAlienSpawner: {isAlienSpawner}, isPlayerSpawner: {isPlayerSpawner}, isLootSpawner: {isLootSpawner}, isSpecial: {isSpecial})";
    }

    public List<List<Tile>> tiles => CalculateTiles();

    List<Tile> openTiles = new();

    public void AddOpenTile(MapPoint point, bool isAlienSpawner = false, bool isPlayerSpawner = false, bool isLootSpawner = false, int roomId = -1, Door.Facing doorFacing = Door.Facing.None, bool ignoreOverlap = false, bool behindDoor = false) {
        if (!openTiles.Any(tile => tile.point.Equals(point))) {
            openTiles.Add(new Tile { point = point, isWall = false, isAlienSpawner = isAlienSpawner, isPlayerSpawner = isPlayerSpawner, isLootSpawner = isLootSpawner, ignoreOverlap = ignoreOverlap, roomId = roomId, doorFacing = doorFacing, behindDoor = behindDoor });
        }
    }

    public bool Overlaps(MapLayout other) {
        int overlaps = 0;
        foreach (var otherTile in other.openTiles) {
            if (otherTile.ignoreOverlap) continue;
            foreach (var tile in openTiles) {
                if (tile.ignoreOverlap) continue;
                if (tile.point.Adjacent(otherTile.point)) {
                    return true;
                }
            }
        }
        return false;
    }
    
    public bool Contains(MapPoint point) {
        return openTiles.Any(tile => tile.point.Equals(point));
    }

    private List<List<Tile>> CalculateTiles() {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach (var point in openTiles.Select(tile => tile.point)) {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }
        var xMod = -minX + 2;
        var yMod = -minY + 2;
        var width = maxX - minX + 5;
        var height = maxY - minY + 5;
        var result = new List<List<Tile>>();
        for (int x = 0; x < width; x++) {
            result.Add(new List<Tile>());
            for (int y = 0; y < height; y++) {
                result[x].Add(new Tile { point = new MapPoint(x, y), isWall = true, roomId = -1});
            }
        }
        foreach (var tile in openTiles) {
            var resultTile = result[tile.point.x + xMod][tile.point.y + yMod];
            resultTile.isWall = false;
            resultTile.isAlienSpawner = tile.isAlienSpawner;
            resultTile.isPlayerSpawner = tile.isPlayerSpawner;
            resultTile.isLootSpawner = tile.isLootSpawner;
            resultTile.roomId = tile.roomId;
            resultTile.doorFacing = tile.doorFacing;
            resultTile.behindDoor = tile.behindDoor;
        }
        return result;
    }
}

public struct MapPoint {
    public int x;
    public int y;
    
    public MapPoint(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static MapPoint operator *(MapPoint a, int num) => new MapPoint(a.x * num, a.y * num);
    public static MapPoint operator /(MapPoint a, int num) => new MapPoint(a.x / num, a.y / num);
    public static MapPoint operator +(MapPoint a, MapPoint b) => new MapPoint(a.x + b.x, a.y + b.y);
    public static MapPoint operator -(MapPoint a, MapPoint b) => new MapPoint(a.x - b.x, a.y - b.y);
    public static MapPoint operator *(MapPoint a, MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.South) return new MapPoint(-a.x, -a.y);
        else if (facing == MapGenerator.Facing.East) return new MapPoint(a.y, -a.x);
        else if (facing == MapGenerator.Facing.West) return new MapPoint(-a.y, a.x);
        else return a;
    }
    public bool Equals(MapPoint other) => x == other.x && y == other.y;
    public bool Adjacent(MapPoint other) => Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y) <= 1;
    public override string ToString() => $"Point({x}, {y})";
    public Vector2 ToVec() => new Vector2(x, y);
    public int manhattanDistance => x + y;

    public MapPoint Mirror(bool mirror = true) => mirror ? new MapPoint(-x, y) : this;
}

public static class MapLayoutExtenstions {

    public static MapGenerator.Facing Opposite(this MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.North) return MapGenerator.Facing.South;
        else if (facing == MapGenerator.Facing.South) return MapGenerator.Facing.North;
        else if (facing == MapGenerator.Facing.West) return MapGenerator.Facing.East;
        else return MapGenerator.Facing.West;
    }

    public static MapPoint ToVector(this MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.North) return new MapPoint(0, 1);
        else if (facing == MapGenerator.Facing.South) return new MapPoint(0, -1);
        else if (facing == MapGenerator.Facing.West) return new MapPoint(-1, 0);
        else return new MapPoint(1, 0);
    }

    public static MapGenerator.Facing RotateBy(this MapGenerator.Facing a, MapGenerator.Facing facing) {
        if (facing == MapGenerator.Facing.South) return a.Opposite();
        else if (facing == MapGenerator.Facing.East) return (MapGenerator.Facing)((((int)a)+1) % 4);
        else if (facing == MapGenerator.Facing.West) return (MapGenerator.Facing)((((int)a)+3) % 4);
        else return a;
    }
}