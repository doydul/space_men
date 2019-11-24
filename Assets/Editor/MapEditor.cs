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
        SceneView.onSceneGUIDelegate = GridUpdate;
    }
    
    public void OnDisable() {
        SceneView.onSceneGUIDelegate = null;
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
            tile.backgroundSprite.sprite = map.corridorSprite;
        } else {
            SetSprite(tile);
        }
        foreach (Tile adjTile in AdjacentTiles(tile)) {
            if (!adjTile.open) SetSprite(adjTile);
        }
    }
    
    private IEnumerable AdjacentTiles(Tile tile) {
        if (tile.gridLocation.x - 1 >= 0) {
            yield return GetTileAt(tile.gridLocation.x - 1, tile.gridLocation.y);
        }
        if (tile.gridLocation.x + 1 < map.width) {
            yield return GetTileAt(tile.gridLocation.x + 1, tile.gridLocation.y);
        }
        if (tile.gridLocation.y - 1 >= 0) {
            yield return GetTileAt(tile.gridLocation.x, tile.gridLocation.y - 1);
        }
        if (tile.gridLocation.y + 1 < map.height) {
            yield return GetTileAt(tile.gridLocation.x, tile.gridLocation.y + 1);
        }
    }
    
    private string WallPattern(Tile tile) {
        var result = "";
        if (tile.gridLocation.x - 1 >= 0) {
            result += GetTileAt(tile.gridLocation.x - 1, tile.gridLocation.y).open ? "0" : "1";
        } else { result += "1"; }
        if (tile.gridLocation.x + 1 < map.width) {
            result += GetTileAt(tile.gridLocation.x + 1, tile.gridLocation.y).open ? "0" : "1";
        } else { result += "1"; }
        if (tile.gridLocation.y - 1 >= 0) {
            result += GetTileAt(tile.gridLocation.x, tile.gridLocation.y - 1).open ? "0" : "1";
        } else { result += "1"; }
        if (tile.gridLocation.y + 1 < map.height) {
            result += GetTileAt(tile.gridLocation.x, tile.gridLocation.y + 1).open ? "0" : "1";
        } else { result += "1"; }
        return result;
    }
    
    private Tile GetTileAt(float x, float y) {
        foreach (Tile tile in map.GetComponentsInChildren<Tile>()) {
            if ((int)x == (int)tile.gridLocation.x && (int)y == (int)tile.gridLocation.y) return tile;
        }
        return null;
    }
    
    private void SetSprite(Tile tile) {
        Sprite sprite = null;
        var background = tile.backgroundSprite.transform;
        switch(WallPattern(tile)) {
            case "1111":
                sprite = map.wallTopSprite;
                break;
                
            case "1011":
                sprite = map.wallSprite;
                background.eulerAngles = new Vector3(0, 0, 0);
                break;
            case "1101":
                sprite = map.wallSprite;
                background.eulerAngles = new Vector3(0, 0, -90);
                break;
            case "0111":
                sprite = map.wallSprite;
                background.eulerAngles = new Vector3(0, 0, 180);
                break;
            case "1110":
                sprite = map.wallSprite;
                background.eulerAngles = new Vector3(0, 0, 90);
                break;
            
            case "1001":
                sprite = map.outerCornerSprite;
                background.eulerAngles = new Vector3(0, 0, 0);
                break;
            case "0101":
                sprite = map.outerCornerSprite;
                background.eulerAngles = new Vector3(0, 0, -90);
                break;
            case "0110":
                sprite = map.outerCornerSprite;
                background.eulerAngles = new Vector3(0, 0, 180); //top left
                break;
            case "1010":
                sprite = map.outerCornerSprite;
                background.eulerAngles = new Vector3(0, 0, 90); // top right
                break;
        }
        tile.backgroundSprite.sprite = sprite;
    }
    
    private Tile MakeTile(Vector2 position, Transform parent) {
        var tileObject = new GameObject();
        var tile = tileObject.AddComponent<Tile>() as Tile;
        tileObject.AddComponent<BoxCollider>();
        var foregroundObject = new GameObject();
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
        tile.gridLocation = position;
        foregroundObject.transform.parent = tileObject.transform;
        foregroundObject.transform.localPosition = new Vector3(0, 0, -2);
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
            columnObject.name = "Column " + x;
            columnObject.transform.parent = map.transform;
            for (int y = 0; y < map.height; y++) {
                tiles[x, y] = MakeTile(new Vector2(x, y), columnObject.transform);
            }
        }
    }
}