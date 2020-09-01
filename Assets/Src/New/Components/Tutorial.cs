using UnityEngine;

public class Tutorial : MonoBehaviour {
    
    public TutorialPanel firstPanel;
    public TutorialPanel secondPanel;
    public TutorialPanel thirdPanel;
    public TutorialPanel fourthPanel;

    void OnSelectSoldier() {
        if (!firstPanel.shown) firstPanel.Show();
    }
    
    void OnMoveSoldier() {
        if (!secondPanel.shown) secondPanel.Show();
    }

    void OnPhaseChange() {
        if (!thirdPanel.shown) thirdPanel.Show();
    }

    public void HandleThirdPanelClosed() {
        if (!fourthPanel.shown) fourthPanel.Show();
    }
}
