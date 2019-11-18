using System.Collections.Generic;
using System.Linq;

public class ViewableState {

    private static ViewableState _instance;

    public static ViewableState instance { get {
        return _instance;
    } }

    public static void Init(
        GamePhase gamePhase,
        CurrentSelectionState currentSelection
    ) {
        _instance = new ViewableState(gamePhase, currentSelection);
    }

    public ViewableState(
        GamePhase gamePhase,
        CurrentSelectionState currentSelection
    ) {
        this.gamePhase = gamePhase;
        this.currentSelection = currentSelection;
    }

    GamePhase gamePhase;
    CurrentSelectionState currentSelection;

    public bool isMovementPhaseActive { get {
        return gamePhase.movement;
    } }

    public List<Tile> selectedTiles { get {
        if (currentSelection.soldierSelected) {
            return new List<Tile>() { currentSelection.GetSelectedSoldier().tile };
        } else {
            return new List<Tile>();
        }
    } }

    public List<Tile> targets { get {
        return currentSelection.GetHighlightedAliens()
               .Select(alien => alien.tile).ToList();
    } }
}
