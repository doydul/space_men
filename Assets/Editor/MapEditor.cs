using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor (typeof(Map))]
public class MapEditor : Editor {
    
    private bool editMode;
    private bool clearTile;
    
    private Map map {
        get {
            return (Map)target;
        }
    }
    
    public void OnEnable() {
        SceneView.duringSceneGui += GridUpdate;
    }
    
    public void OnDisable() {
        SceneView.duringSceneGui -= GridUpdate;
        editMode = false;
    }
    
    public void OnSceneGUI() {
        if (editMode) {
            int id = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(id);
        }
    }
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        if (GUILayout.Button(" Generate ")) {
            Generate();
        }
        
        if (!editMode && GUILayout.Button(" Edit Mode ")) {
            editMode = true;
        } else if (editMode && GUILayout.Button(" Disable Edit Mode ")) {
            editMode = false;
        }
        
        SceneView.RepaintAll();
    }
    
    void GridUpdate(SceneView sceneview) {
        if (!editMode) return;
        Event e = Event.current;
        if (e.button == 0 && e.type == EventType.MouseDown) {
            e.Use();
            var tile = TileAt(e.mousePosition);
            if (tile != null) {
              if (Event.current.shift) {
                  AddSpawner(tile);
              } else if (Event.current.control) {
                  AddStartLocation(tile);
              } else {
                  FlipTile(tile);
                  clearTile = tile.open;
              }  
            }
        }
        
        if (e.button == 0 && e.type == EventType.MouseDrag) {
            var tile = TileAt(e.mousePosition);
            if (tile != null && tile.open != clearTile) { 
                FlipTile(tile);
            }
        }
    }
    
    private void AddSpawner(Tile tile) {
        var spawner = tile.GetComponent<Spawner>();
        if (spawner != null) {
            DestroyImmediate(spawner);
        } else {
            tile.gameObject.AddComponent<Spawner>();
        }
    }
    
    private void AddStartLocation(Tile tile) {
        var startLocation = tile.GetComponent<StartLocation>();
        if (startLocation != null) {
            DestroyImmediate(startLocation);
        } else {
            tile.gameObject.AddComponent<StartLocation>();
        }
    }
    
    private Tile TileAt(Vector2 mousePosition) {
        RaycastHit hit;
        Physics.Raycast(HandleUtility.GUIPointToWorldRay(mousePosition), out hit);
        if (hit.collider != null) { 
            var tile = hit.collider.gameObject.GetComponent<Tile>();
            return tile;
        }
        return null;
    }
    
    private void FlipTile(Tile tile) {
        tile.open = !tile.open;
        if (tile.open) {
            tile.backgroundSprite.sprite = map.corridorSprites.WeightedSelect().sprite;
        } else {
            SetSprite(tile);
        }
        foreach (Tile adjTile in AdjacentTiles(tile)) {
            if (!adjTile.open) SetSprite(adjTile);
        }
    }
    
    private IEnumerable AdjacentTiles(Tile tile) {
        for (int y = 0; y < 3; y++) {
            for (int x = 0; x < 3; x++) {
                int realx = (int)tile.gridLocation.x + x - 1;
                int realy = (int)tile.gridLocation.y + y - 1;
                if (realx >= 0 && realx < map.width && realy >= 0 && realy < map.height) {
                    yield return GetTileAt(realx, realy);
                }
            }
        }
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

    private bool IsWallAt(float x, float y) {
        if (x >= 0 && x < map.width && y >= 0 && y < map.height) {
            return !GetTileAt(x, y).open;
        } else {
            return true;
        }
    }
    
    private Tile GetTileAt(float x, float y) {
        foreach (Tile tile in map.GetComponentsInChildren<Tile>()) {
            if ((int)x == (int)tile.gridLocation.x && (int)y == (int)tile.gridLocation.y) return tile;
        }
        return null;
    }

    // 678
    // 345
    // 012

    private void SetSprite(Tile tile) {
        Sprite sprite = null;
        var background = tile.backgroundSprite.transform;

        if (!IsWallAtIndex(tile, 5) && !IsWallAtIndex(tile, 1)) {
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
    
    private Tile MakeTile(Vector2 position, Transform parent) {
        var tileObject = new GameObject();
        var tile = tileObject.AddComponent<Tile>() as Tile;
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
        
        return tile;
    }
    
    private Vector3 SpriteScale(Sprite sprite) {
        float xScale = sprite.pixelsPerUnit / sprite.rect.width;
        float yScale = sprite.pixelsPerUnit / sprite.rect.height;
        return new Vector3(xScale, yScale, 1);
    }
    
    private void Generate() {
        Tile[,] tiles = new Tile[map.width, map.height];
        for (int x = 0; x < map.width; x++) {
            var columnObject = new GameObject().transform;
            columnObject.localPosition = Vector3.zero;
            columnObject.name = "Column " + x;
            columnObject.transform.parent = map.transform;
            columnObject.localPosition = Vector3.zero;
            for (int y = 0; y < map.height; y++) {
                tiles[x, y] = MakeTile(new Vector2(x, y), columnObject.transform);
            }
        }
    }
}