using UnityEngine;
using TMPro;

public class CampaignUI : MonoBehaviour {
    
    public static CampaignUI instance;

    public BenchComponent bench;
    public WorkshopComponent workshop;
    public SquadSoldierIcon[] soldierIcons;
    public TMP_Text soldierInfoText;
    public GameObject equipWeaponButton;
    public GameObject equipArmourButton;
    public GameObject weaponButton;
    public GameObject weaponButtonHighlight;
    public GameObject armourButton;
    public GameObject armourButtonHighlight;
    
    MetaSoldier selectedSoldier;

    void Awake() => instance = this;

    void Start() {
        DisplaySquad();
        soldierInfoText.text = "";
        weaponButton.SetActive(false);
        armourButton.SetActive(false);
    }
    
    void Update() {
        if (workshop.activeItem != null && selectedSoldier != null) {
            weaponButtonHighlight.SetActive(false);
            armourButtonHighlight.SetActive(false);
            if (workshop.activeItem.isWeapon) {
                equipWeaponButton.SetActive(true);
                equipArmourButton.SetActive(false);
            } else {
                equipWeaponButton.SetActive(false);
                equipArmourButton.SetActive(true);
            }
        } else {
            equipWeaponButton.SetActive(false);
            equipArmourButton.SetActive(false);
        }
    }

    public void DisplaySquad() {
        int i = 0;
        foreach (var soldierIcon in soldierIcons) {
            if (PlayerSave.current.squad.SlotOccupied(i)) soldierIcon.ShowSoldier();
            else soldierIcon.HideSoldier();
            i++;
        }
    }
    
    public void SelectSoldier(int squadSlotId) {
        if (soldierIcons[squadSlotId].selected) {
            bench.Open(squadSlotId);
        } else {
            RefreshSoldier(squadSlotId);
        }
    }
    
    public void RefreshSoldier(int squadSlotId) {
        foreach (var icon in soldierIcons) icon.Deselect();
        soldierIcons[squadSlotId].Select();
        weaponButtonHighlight.SetActive(false);
        armourButtonHighlight.SetActive(false);
        if (workshop.activeItem == null && workshop.activeBlueprint == null) workshop.DisplayNonInventoryItemInfo(null);
        if (PlayerSave.current.squad.SlotOccupied(squadSlotId)) {
            selectedSoldier = PlayerSave.current.squad.GetMetaSoldier(squadSlotId);
            soldierInfoText.text = selectedSoldier.GetFullDescription();
            weaponButton.SetActive(true);
            weaponButton.GetComponentInChildren<TMP_Text>().text = selectedSoldier.weapon.name;
            armourButton.SetActive(true);
            armourButton.GetComponentInChildren<TMP_Text>().text = selectedSoldier.armour.name;
        } else {
            selectedSoldier = null;
            soldierInfoText.text = "";
            weaponButton.SetActive(false);
            armourButton.SetActive(false);
        }
    }
    
    public void ClickWeapon() {
        workshop.DisplayNonInventoryItemInfo(selectedSoldier.weapon);
        weaponButtonHighlight.SetActive(true);
        armourButtonHighlight.SetActive(false);
    }
    
    public void ClickArmour() {
        workshop.DisplayNonInventoryItemInfo(selectedSoldier.armour);
        armourButtonHighlight.SetActive(true);
        weaponButtonHighlight.SetActive(false);
    }
    
    public void EquipSelectedItem() {
        PlayerSave.current.inventory.Equip(selectedSoldier, workshop.activeItem);
        workshop.activeItem = null;
        workshop.DisplayInventoryItems();
        weaponButton.GetComponentInChildren<TMP_Text>().text = selectedSoldier.weapon.name;
        armourButton.GetComponentInChildren<TMP_Text>().text = selectedSoldier.armour.name;
    }
}