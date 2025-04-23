using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MapInstantiator : MonoBehaviour {
    
    [System.Serializable]
    public class BlueprintCollection {
        public List<Blueprint> blueprints;
    }
    
    [System.Serializable]
    public class Blueprint {
        public int vents;
        public int loots;
        public int corridors;
        public int secondaryCorridors;
        public int rooms;
    }

    public static bool skipGenerate;
    public static bool forceTerminalObjective;
    public static bool forceHoldObjective;
    
    public static MapInstantiator instance;
    void Awake() => instance = this;

    public List<MapBlueprint> blueprints;
    public Map map;

    public void Generate(float difficulty) {
        if (skipGenerate) return;
        
        for (int i = map.transform.childCount - 1; i >= 0; i--) {
            var child = map.transform.GetChild(i);
            if (child.name.Contains("Column")) {
                DestroyImmediate(child.gameObject);
            }
        }
        var blueprint = blueprints[(int)(difficulty * 2)];
        var objectives = PlayerSave.current.mission.GetObjectives();
        
        // debug
        if (forceTerminalObjective) objectives.Add(new ActivateTerminal { required = true });
        if (forceHoldObjective) objectives.Add(new WaveDefence { required = true });
        //
        
        var mapLayout = new MapGenerator(new MapGenerator.Blueprint {
            seed = PlayerSave.current.levelSeed,
            corridors = blueprint.corridors,
            secondaryCorridors = blueprint.secondaryCorridors,
            loops = blueprint.loops,
            rooms = blueprint.rooms,
            objectives = objectives
        }).Generate();
        var tiles = new List<Tile>();
        var ventTiles = new List<Tile>();
        var mapLayoutCache = mapLayout.tiles;
        int startRoomId = mapLayoutCache.SelectMany(x => x).Where(tile => tile.roomId > 0 && !objectives.Any(obj => obj.roomId == tile.roomId)).Select(tile => tile.roomId).Min();
        int remainingVents = blueprint.vents;
        for (int x = 0; x < mapLayoutCache.Count; x++) {
            var columnObject = new GameObject().transform;
            columnObject.transform.parent = map.transform;
            columnObject.name = "Column " + x;
            columnObject.localPosition = Vector3.zero;
            for (int y = 0; y < mapLayoutCache[0].Count; y++) {
                var tileData = mapLayoutCache[x][y];
                var tile = MakeTile(new Vector2(x, y), columnObject.transform, !tileData.isWall);
                
                if (!tileData.isWall) { // add vents to any 'end' tiles
                    int neighbours = 0;
                    if (!mapLayoutCache[x - 1][y].isWall) neighbours++;
                    if (!mapLayoutCache[x + 1][y].isWall) neighbours++;
                    if (!mapLayoutCache[x][y - 1].isWall) neighbours++;
                    if (!mapLayoutCache[x][y + 1].isWall) neighbours++;
                    if (neighbours == 1) {
                        remainingVents--;
                        tile.gameObject.AddComponent<Spawner>();
                        var vent = Instantiate(Resources.Load<Transform>("Prefabs/Vent"));
                        tile.SetActor(vent, true);
                    }
                }
                
                if (tileData.isAlienSpawner) {
                    if (tileData.roomId == startRoomId) tile.gameObject.AddComponent<StartLocation>();
                    else ventTiles.Add(tile);
                }
                
                if (tileData.doorFacing != Door.Facing.None) MakeDoor(tile, tileData.doorFacing);
                
                tiles.Add(tile);

                // Add or update room
                if (tileData.roomId >= 0) {
                    if (!map.rooms.ContainsKey(tileData.roomId)) {
                        map.rooms[tileData.roomId] = new Map.Room { id = tileData.roomId, start = tileData.roomId == startRoomId };
                    }
                    map.rooms[tileData.roomId].tiles.Add(tile);
                }
            }
        }
        map.width = mapLayoutCache.Count;
        map.height = mapLayoutCache[0].Count;
        map.ClearTiles();
        foreach (var tile in tiles) {
            SetSprites(tile);
        }
        
        // add vents
        while (ventTiles.Any() && remainingVents > 0) {
            var ventTile = ventTiles.Sample();
            ventTiles.Remove(ventTile);
            ventTile.gameObject.AddComponent<Spawner>();
            var vent = Instantiate(Resources.Load<Transform>("Prefabs/Vent"));
            ventTile.SetActor(vent, true);
            remainingVents--;
        }
        
        // add common loot
        int remainingLoot = blueprint.loots;
        while (ventTiles.Any() && remainingLoot > 0) {
            var ventTile = ventTiles.Sample();
            ventTiles.Remove(ventTile);
            
            var loot = LootGenerator.instance.MakeCommonLoot(PlayerSave.current.difficulty);
            LootGenerator.instance.InstantiateLootChest(loot, ventTile.gridLocation, true);
            remainingLoot--;
        }
        
        Objectives.AddToMap(map, objectives, map.rooms.FirstOrDefault(roomKV => roomKV.Value.start).Value);
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
        var backGroundSpriteObjects = new List<GameObject>();
        for (int i = 0; i < 4; i++ ) {
            var go = new GameObject();
            backGroundSpriteObjects.Add(go);
            go.transform.parent = backgroundObject.transform;
            go.transform.localPosition = new Vector3(new [] { -0.25f, 0.25f, 0.25f, -0.25f }[i], new [] { 0.25f, 0.25f, -0.25f, -0.25f }[i], 0);
            Debug.Log(go.transform.localPosition);
            go.AddComponent<SpriteRenderer>().sprite = map.wallTopSprite;
            go.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        tile.backgroundSprite = backgroundObject.AddComponent<SpriteRenderer>();
        
        var highlightSpriteRenderer = highlightObject.AddComponent<SpriteRenderer>();
        var fogSpriteRenderer = fogObject.AddComponent<SpriteRenderer>();
        highlightSpriteRenderer.enabled = false;
        fogSpriteRenderer.enabled = false;
        
        tileObject.name = "Tile " + position.x + ", " + position.y;
        tileObject.transform.parent = parent;
        tileObject.transform.localPosition = position;
        tile.backgroundSprites = backGroundSpriteObjects.Select(go => go.GetComponent<SpriteRenderer>()).ToArray();
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
    
    private Door MakeDoor(Tile tile, Door.Facing facing) {
        var doorTrans = Instantiate(Resources.Load<Transform>("Prefabs/Door"));
        var door = doorTrans.GetComponent<Door>();
        door.SetFacing(facing);
        tile.SetActor(doorTrans, true);
        return door;
    }

    private void SetSprites(Tile tile) {
        Sprite sprite = null;
        
        if (tile.open) {
            sprite = map.corridorSprites.WeightedSelect().sprite;
            var tmp = tile.backgroundSprite.transform.localScale;
            if (Random.value < 0.5f) tmp.y = -1;
            if (Random.value < 0.5f) tmp.x = -1;
            tile.backgroundSprite.transform.localScale = tmp;
            if (Random.value < 0.05f) tile.backgroundSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            foreach (var renderer in tile.backgroundSprites) renderer.enabled = false;
        } else {
            tile.backgroundSprite.enabled = false;
            
            for (int i = 0; i < 4; i++) {
                // int x = i % 2;
                // int y = i / 2;
                var pos = tile.gridLocation;
                var offset = new Vector2(new[] { 0, 1, 0, -1 }[i], new[] { 1, 0, -1, 0 }[i]);
                var transpose = new Vector2(-offset.y, offset.x);
                var background = tile.backgroundSprites[i];
                background.transform.eulerAngles = new Vector3(0, 0, i * -90);
                
                if (!IsWallAt(pos + offset) && !IsWallAt(pos + transpose)) {
                    background.sprite = map.outerCornerSprite;
                } else if (!IsWallAt(pos + offset)) {
                    background.sprite = map.wallSprite;
                    background.transform.rotation *= Quaternion.Euler(0, 0, -90);
                } else if (!IsWallAt(pos + transpose)) {
                    background.sprite = map.wallSprite;
                } else if (!IsWallAt(pos + transpose + offset)) {
                    background.sprite = map.innerCornerSprite;
                } else {
                    background.enabled = false;
                }
            }
        }
        tile.backgroundSprite.sprite = sprite;
    }

    private bool IsWallAt(float x, float y) {
        return IsWallAt(new Vector2(x, y));
    }
    private bool IsWallAt(Vector2 pos) {
        var tile = map.GetTileAt(pos);
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