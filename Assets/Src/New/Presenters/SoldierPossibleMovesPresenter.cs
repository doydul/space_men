using UnityEngine;
using System.Linq;

using Data;

public class SoldierPossibleMovesPresenter : Presenter, IPresenter<SoldierPossibleMovesOutput> {
  
    public static SoldierPossibleMovesPresenter instance { get; private set; }
  
    public MapHighlighter highlighter;
    public Map map;
    
    PossibleMoveLocation[] possibleMoves;
    
    public void Present(SoldierPossibleMovesOutput input) {
        possibleMoves = input.possibleMoveLocations;
        highlighter.ClearHighlights();
        foreach (var moveLocation in input.possibleMoveLocations) {
            var color = moveLocation.sprint ? Color.yellow : Color.green;
            highlighter.HighlightTile(map.GetTileAt(new Vector2(moveLocation.position.x, moveLocation.position.y)), color);
        }
    }
    
    public PossibleMoveLocation[] GetPossibleMoves() {
        return possibleMoves;
    }
    
    public bool ValidMoveLocation(Vector2 gridlocation) {
        return possibleMoves.Any(moveLocation => moveLocation.position.x == (int)gridlocation.x && moveLocation.position.y == (int)gridlocation.y);
    }
    
    void Awake() {
        instance = this;
    }
}
