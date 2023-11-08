using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public partial class Map : MonoBehaviour {
    
    public static Map instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public Sprite wallSprite;
    public Sprite innerCornerSprite;
    public Sprite outerCornerSprite;
    public Sprite corridorSprite;
    public Sprite wallTopSprite;
    public Sprite highlightSprite;
    public Sprite fogSprite;

    public Spawner[] spawners { get { return GetComponentsInChildren<Spawner>(); } }
    public StartLocation[] startLocations { get { return GetComponentsInChildren<StartLocation>(); } }

    public int width;
    public int height;

    private Tile[,] _tiles;
    public Tile[,] tiles { get {
        if (_tiles == null) {
            _tiles = new Tile[width, height];
            foreach (Tile tile in GetComponentsInChildren<Tile>()) {
                _tiles[(int)tile.gridLocation.x, (int)tile.gridLocation.y] = tile;
            }
        }
        return _tiles;
    } }

    public List<T> GetActors<T>() {
        var result = new List<T>();
        foreach (Tile tile in EnumerateTiles()) {
            T actor = tile.GetActor<T>();
            if (actor != null) result.Add(actor);
            T backgroundActor = tile.GetBackgroundActor<T>();
            if (backgroundActor != null) result.Add(backgroundActor);
        }
        return result;
    }

    public T GetActorAt<T>(Vector2 gridLocation) {
        return GetTileAt(gridLocation).GetActor<T>();
    }

    public Tile GetTileAt(Vector2 gridLocation) {
        int x = (int)gridLocation.x;
        int y = (int)gridLocation.y;
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1)) return null;
        return tiles[x, y];
    }

    public Actor GetActorByID(string id) {
        foreach (var actor in GetActors<Actor>()) {
            if (actor.id == id) return actor;
        }
        throw new System.Exception("Actor could not be found");
    }

    public Actor GetActorByIndex(long index) {
        foreach (var actor in GetActors<Actor>()) {
            if (actor.index == index) return actor;
        }
        throw new System.Exception("Actor could not be found");
    }

    public IEnumerable<Tile> EnumerateTiles() {
        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                yield return tiles[x, y];
            }
        }
    }

    public IEnumerable<Tile> AdjacentTiles(Tile tile, bool includeDiagonal = false) {
        var adjTile = GetTileAt(new Vector2(tile.gridLocation.x - 1, tile.gridLocation.y));
        if (adjTile != null) yield return adjTile;
        adjTile = GetTileAt(new Vector2(tile.gridLocation.x + 1, tile.gridLocation.y));
        if (adjTile != null) yield return adjTile;
        adjTile = GetTileAt(new Vector2(tile.gridLocation.x, tile.gridLocation.y - 1));
        if (adjTile != null) yield return adjTile;
        adjTile = GetTileAt(new Vector2(tile.gridLocation.x, tile.gridLocation.y + 1));
        if (adjTile != null) yield return adjTile;
        if (includeDiagonal) {
            adjTile = GetTileAt(new Vector2(tile.gridLocation.x + 1, tile.gridLocation.y + 1));
            if (adjTile != null) yield return adjTile;
            adjTile = GetTileAt(new Vector2(tile.gridLocation.x + 1, tile.gridLocation.y - 1));
            if (adjTile != null) yield return adjTile;
            adjTile = GetTileAt(new Vector2(tile.gridLocation.x - 1, tile.gridLocation.y + 1));
            if (adjTile != null) yield return adjTile;
            adjTile = GetTileAt(new Vector2(tile.gridLocation.x - 1, tile.gridLocation.y - 1));
            if (adjTile != null) yield return adjTile;
        }
    }

    public IEnumerable<Tile> AdjacentTiles(Vector2 gridLocation, bool includeDiagonal = false) {
        return AdjacentTiles(GetTileAt(gridLocation), includeDiagonal);
    }
}
