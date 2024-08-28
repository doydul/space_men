using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoldierIcon : MonoBehaviour {
    
    public Image image;
    public GameObject highlight;
    public TMP_Text infoText;
    
    Soldier soldier;
    int updateCounter;
    
    void Update() {
        UpdateInfo();
    }
    
    void UpdateInfo() {
        // if (soldier != null) infoText.text = $"hp: {soldier.health}/{soldier.maxHealth}\nw: {soldier.weapon.name}\na: {soldier.armour.name}";
        if (soldier != null) {
            infoText.text = $"hp: {soldier.health}/{soldier.maxHealth}";
            highlight.SetActive(UIState.instance.GetSelectedActor() == soldier);
        }
    }
    
    public void ShowSoldier(Soldier soldier) {
        this.soldier = soldier;
        UpdateInfo();
    }
    
    public void Select() {
        if (UIState.instance.IsActorSelected()) UIState.instance.GetSelectedActor().Deselect();
        soldier.Select();
    }
}