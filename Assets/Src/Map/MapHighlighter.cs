using System.Collections.Generic;

public class MapHighlighter {
    
    private List<Tile> highlightedTiles;
    
    public MapHighlighter() {
        highlightedTiles = new List<Tile>();
    }
    
    public void ClearHighlights() {
        foreach (var tile in highlightedTiles) {
            tile.ClearHighlight();
        }
        highlightedTiles.Clear();
    }
    
    public void HighlightTile(Tile tile) {
        tile.Highlight();
        highlightedTiles.Add(tile);
    }
}