using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CampaignUI : MonoBehaviour {
    
    public static CampaignUI instance;

    public BenchComponent bench;
    public SquadSoldierIcon[] soldierIcons;
    public TMP_Text soldierInfoText;
    public GameObject equipWeaponButton;
    public GameObject equipArmourButton;

    void Awake() => instance = this;

    void Start() {
        DisplaySquad();
    }

    public void DisplaySquad() {
        int i = 0;
        foreach (var soldierIcon in soldierIcons) {
            if (PlayerSave.current.squad.SlotOccupied(i)) soldierIcon.ShowSoldier();
            else soldierIcon.HideSoldier();
            i++;
        }
    }

    public void NextLevel() {
        SceneManager.LoadScene("Mission");
    }
    
    public void SelectSoldier(int squadSlotId) {
        if (soldierIcons[squadSlotId].selected) {
            bench.Open(squadSlotId);
        } else {
            foreach (var icon in soldierIcons) icon.Deselect();
            soldierIcons[squadSlotId].Select();
        }
        if (PlayerSave.current.squad.SlotOccupied(squadSlotId)) {
            soldierInfoText.text = PlayerSave.current.squad.GetMetaSoldier(squadSlotId).GetFullDescription();
        } else {
            soldierInfoText.text = "";
        }
    }
}