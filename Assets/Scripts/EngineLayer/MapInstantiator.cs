using UnityEngine;
using System.Collections.Generic;

public class MapInstantiator : MonoBehaviour {

    public static MapInstantiator instance;
    void Awake() => instance = this;

    public Map map;

    public void Generate() {
        for (int i = map.transform.childCount - 1; i >= 0; i--) {
            var child = map.transform.GetChild(i);
            if (child.name.Contains("Column")) {
                DestroyImmediate(child.gameObject);
            }
        }
        var mapLayout = new MapGenerator().Generate();
        var tiles = new List<Tile>();
        for (int x = 0; x < mapLayout.tiles.Count; x++) {
            var columnObject = new GameObject().transform;
            columnObject.transform.parent = map.transform;
            columnObject.name = "Column " + x;
            columnObject.localPosition = Vector3.zero;
            for (int y = 0; y < mapLayout.tiles[0].Count; y++) {
                var tileData = mapLayout.tiles[x][y];
                var tile = MakeTile(new Vector2(x, y), columnObject.transform, !tileData.isWall);
                if (tileData.isAlienSpawner) {
                    tile.gameObject.AddComponent<Spawner>();
                    var vent = Instantiate(Resources.Load<Transform>("Prefabs/Vent"));
                    tile.SetActor(vent, true);
                }
                if (tileData.isPlayerSpawner) tile.gameObject.AddComponent<StartLocation>();
                if (tileData.isLootSpawner) tile.gameObject.AddComponent<LootSpawner>();
                tiles.Add(tile);

                // Add or update room
                if (tileData.roomId >= 0) {
                    if (!map.rooms.ContainsKey(tileData.roomId)) {
                        map.rooms[tileData.roomId] = new Map.Room { id = tileData.roomId };
                    }
                    map.rooms[tileData.roomId].tiles.Add(tile);
                }
            }
        }
        map.width = mapLayout.tiles.Count;
        map.height = mapLayout.tiles[0].Count;
        map.ClearTiles();
        foreach (var tile in tiles) {
            SetSprite(tile);
        }
    }

    private Tile MakeTile(Vector2 position, Transform parent, bool open) {
        var tileObject = new GameObject();
        var tile = tileObject.AddComponent<Tile>() as Tile;
        tile.open = open;
        if (!open) tile.gameObject.layer = LayerMask.NameToLayer("Walls");
        tileObject.AddComponent<BoxCollider>();
        var foregroundObject = new GameObject();
        var midgroundObject = new GameObject();
        var backgroundObject = new GameObject();
        var highlightObject = new GameObject();
        var fogObject = new GameObject();
        var backgroundSpriteRenderer = backgroundObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        var highlightSpriteRenderer = highlightObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        var fogSpriteRenderer = fogObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        highlightSpriteRenderer.enabled = false;
        fogSpriteRenderer.enabled = false;
        
        tileObject.name = "Tile " + position.x + ", " + position.y;
        tileObject.transform.parent = parent;
        tileObject.transform.localPosition = position;
        tile.backgroundSprite = backgroundSpriteRenderer;
        backgroundSpriteRenderer.sprite = map.wallTopSprite;
        backgroundObject.transform.localScale = SpriteScale(map.wallTopSprite);
        tile.highlightSprite = highlightSpriteRenderer;
        highlightSpriteRenderer.sprite = map.highlightSprite;
        highlightObject.transform.localScale = SpriteScale(map.highlightSprite);
        tile.fogSprite = fogSpriteRenderer;
        fogSpriteRenderer.sprite = map.fogSprite;
        fogObject.transform.localScale = SpriteScale(map.fogSprite);
        tile.foreground = foregroundObject.transform;
        tile.midground = midgroundObject.transform;
        tile.gridLocation = position;
        foregroundObject.transform.parent = tileObject.transform;
        foregroundObject.transform.localPosition = new Vector3(0, 0, -2);
        midgroundObject.transform.parent = tileObject.transform;
        midgroundObject.transform.localPosition = new Vector3(0, 0, -1.5f);
        backgroundObject.transform.parent = tileObject.transform;
        backgroundObject.transform.localPosition = new Vector3(0, 0, 0);
        highlightObject.transform.parent = tileObject.transform;
        highlightObject.transform.localPosition = new Vector3(0, 0, -1);
        fogObject.transform.parent = tileObject.transform;
        fogObject.transform.localPosition = new Vector3(0, 0, -3);
        
        tile.Init();
        return tile;
    }

    private void SetSprite(Tile tile) {
        Sprite sprite = null;
        var background = tile.backgroundSprite.transform;

        if (tile.open) {
            sprite = map.corridorSprite;
        } else if (!IsWallAtIndex(tile, 5) && !IsWallAtIndex(tile, 1)) {
            sprite = map.outerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -0);
        } else if (!IsWallAtIndex(tile, 1) && !IsWallAtIndex(tile, 3)) {
            sprite = map.outerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -90);
        } else if (!IsWallAtIndex(tile, 3) && !IsWallAtIndex(tile, 7)) {
            sprite = map.outerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -180);
        } else if (!IsWallAtIndex(tile, 7) && !IsWallAtIndex(tile, 5)) {
            sprite = map.outerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -270);
        } else if (!IsWallAtIndex(tile, 5)) {
            sprite = map.wallSprite;
            background.eulerAngles = new Vector3(0, 0, -0);
        } else if (!IsWallAtIndex(tile, 1)) {
            sprite = map.wallSprite;
            background.eulerAngles = new Vector3(0, 0, -90);
        } else if (!IsWallAtIndex(tile, 3)) {
            sprite = map.wallSprite;
            background.eulerAngles = new Vector3(0, 0, -180);
        } else if (!IsWallAtIndex(tile, 7)) {
            sprite = map.wallSprite;
            background.eulerAngles = new Vector3(0, 0, -270);
        } else if (!IsWallAtIndex(tile, 8)) {
            sprite = map.innerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -0);
        } else if (!IsWallAtIndex(tile, 2)) {
            sprite = map.innerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -90);
        } else if (!IsWallAtIndex(tile, 0)) {
            sprite = map.innerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -180);
        } else if (!IsWallAtIndex(tile, 6)) {
            sprite = map.innerCornerSprite;
            background.eulerAngles = new Vector3(0, 0, -270);
        } else {
            sprite = map.wallTopSprite;
        }
        tile.backgroundSprite.sprite = sprite;
    }

    private bool IsWallAt(int x, int y) {
        var tile = map.GetTileAt(new Vector2(x, y));
        return tile == null ? true : !tile.open;
    }

    private Vector3 SpriteScale(Sprite sprite) {
        float xScale = sprite.pixelsPerUnit / sprite.rect.width;
        float yScale = sprite.pixelsPerUnit / sprite.rect.height;
        return new Vector3(xScale, yScale, 1);
    }

    private string WallPattern(Tile tile) {
        var result = "";
        for (int y = 0; y < 3; y++) {
            for (int x = 0; x < 3; x++) {
                int realx = (int)tile.gridLocation.x + x - 1;
                int realy = (int)tile.gridLocation.y + y - 1;
                result += IsWallAt(realx, realy) ? "1" : "0";
            }
        }
        return result;
    }

    private bool IsWallAtIndex(Tile tile, int index) {
        var pattern = WallPattern(tile);
        return pattern[index] == '1';
    }
}