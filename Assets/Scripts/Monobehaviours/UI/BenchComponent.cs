using UnityEngine;
using TMPro;

public class BenchComponent : MonoBehaviour {
    
    public Transform soldierPrototype;
    public TMP_Text hireButtonText;
    public TMP_Text soldierInfoText;
    
    int squadPositionId;
    MetaSoldier activeBenchSoldier;
    
    const int hireCost = 300;
    
    void Start() {
        soldierPrototype.gameObject.SetActive(false);
        Close();
    }
    
    public void Open(int squadPositionId) {
        soldierInfoText.text = "";
        activeBenchSoldier = null;
        this.squadPositionId = squadPositionId;
        DisplayBenchSoldiers();
        DisplayHireButtonText();
        gameObject.SetActive(true);
    }
    
    public void Close() {
        gameObject.SetActive(false);
        CampaignUI.instance.DisplaySquad();
    }
    
    public void Confirm() {
        if (activeBenchSoldier != null) {
            var previousSoldier = PlayerSave.current.squad.RemoveMetaSoldier(squadPositionId);
            if (previousSoldier != null) PlayerSave.current.bench.Add(previousSoldier);
            PlayerSave.current.squad.AddMetaSoldier(activeBenchSoldier);
            PlayerSave.current.bench.Remove(activeBenchSoldier);
            Close();
        }
    }
    
    public void HireSoldier() {
        Debug.Log(PlayerSave.current.credits);
        if (PlayerSave.current.credits >= hireCost) {
            PlayerSave.current.credits -= hireCost;
            PlayerSave.current.bench.Add(MetaSquad.GenerateDefaultSoldier());
            DisplayBenchSoldiers();
            DisplayHireButtonText();
        }
    }
    
    public void ClickBenchedSoldier(int index) {
        activeBenchSoldier = PlayerSave.current.bench[index];
        DisplaySoldierInfo(activeBenchSoldier);
    }
    
    void DisplayBenchSoldiers() {
        soldierPrototype.parent.DestroyChildren(1);
        
        int index = 0;
        foreach (var metaSoldier in PlayerSave.current.bench) {
            var soldierTrans = Instantiate(soldierPrototype, soldierPrototype.parent);
            soldierTrans.gameObject.SetActive(true);
            var textComponent = soldierTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = $"{metaSoldier.weapon.name} {metaSoldier.armour.name}";
            var buttonComponent = soldierTrans.GetComponentInChildren<ButtonHandler>();
            int soldierIndex = index;
            buttonComponent.action.AddListener(() => {
                ClickBenchedSoldier(soldierIndex);
            });
            index++;
        }
    }
    
    void DisplaySoldierInfo(MetaSoldier soldier) {
        soldierInfoText.text = soldier.GetFullDescription();
    }
    
    void DisplayHireButtonText() {
        hireButtonText.text = $"hire soldier {PlayerSave.current.credits}/{hireCost}";
    }
}