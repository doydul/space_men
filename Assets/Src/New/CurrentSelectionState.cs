using System.Collections.Generic;

public class CurrentSelectionState {

    Soldier selectedSoldier;
    List<Alien> highlightedAliens;

    public bool soldierSelected { get { return selectedSoldier != null; } }

    public CurrentSelectionState() {
        highlightedAliens = new List<Alien>();
    }

    public void SelectSoldier(Soldier soldier) {
        selectedSoldier = soldier;
    }

    public void DeselectSoldier() {
        selectedSoldier = null;
    }

    public Soldier GetSelectedSoldier() {
        return selectedSoldier;
    }

    public List<Alien> GetHighlightedAliens() {
        return highlightedAliens;
    }
}
