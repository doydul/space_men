using UnityEngine;

public class UIManager : MonoBehaviour {
    
    static UIManager instance;
    
    public GameObject objectivesButton;
    public ObjectivesPanel objectivesPanel;
    public SoldierIconHeader soldierIcons;
    
    void Awake() => instance = this;
    
    public static void HideNormalGameHud() {
        instance.objectivesPanel.Hide();
        instance.objectivesButton.SetActive(false);
        instance.soldierIcons.Hide();
    }
    
    public static void ShowNormalGameHud() {
        instance.objectivesPanel.Hide();
        instance.soldierIcons.Show();
    }
}