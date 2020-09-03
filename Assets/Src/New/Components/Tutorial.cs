using UnityEngine;

public class Tutorial : MonoBehaviour {
    
    public TutorialPanel firstPanel;
    public TutorialPanel secondPanel;
    public TutorialPanel thirdPanel;
    public TutorialPanel fourthPanel;
    public ScriptingController controller;
    public Tile alienSpawnTile;

    void OnSelectSoldier() {
        if (!firstPanel.shown) firstPanel.Show();
    }
    
    void OnMoveSoldier() {
        if (!secondPanel.shown) secondPanel.Show();
    }

    void OnPhaseChange() {
        if (!thirdPanel.shown) thirdPanel.Show();
    }

    void OnMissionStart() {
        controller.SpawnAliens("Alien", (int)alienSpawnTile.gridLocation.x, (int)alienSpawnTile.gridLocation.y, Data.Direction.Up);
    }

    public void HandleThirdPanelClosed() {
        if (!fourthPanel.shown) fourthPanel.Show();
    }
}
