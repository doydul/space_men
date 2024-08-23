using UnityEngine;
using System.Linq;
using TMPro;

public class SoldierComponent : MonoBehaviour {

    public TMP_Text weaponText;
    public TMP_Text armourText;
    public TMP_Text descriptionText;
    public Transform inventoryItemPrototype;
    public GameObject weaponEquipButton;
    public GameObject armourEquipButton;

    MetaSoldier soldier;
    InventoryItem selectedItem;
    
    void Start() {
        inventoryItemPrototype.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Open(MetaSoldier soldier) {
        this.soldier = soldier;
        selectedItem = null;
        gameObject.SetActive(true);
        weaponEquipButton.SetActive(false);
        armourEquipButton.SetActive(false);
        weaponText.text = soldier.weapon.name;
        armourText.text = soldier.armour.name;
        descriptionText.text = "";
    }

    void DisplayItems(InventoryItem.Type type) {
        inventoryItemPrototype.parent.DestroyChildren(1);
        foreach (var item in PlayerSave.current.inventory.items.Where(el => el.type == type)) {
            var itemTrans = Instantiate(inventoryItemPrototype, inventoryItemPrototype.parent);
            itemTrans.gameObject.SetActive(true);
            var textComponent = itemTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = item.name;
            var buttonComponent = itemTrans.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                descriptionText.text = DescriptionFor(item);
                selectedItem = item;
                if (item.isWeapon) weaponEquipButton.SetActive(true);
                else armourEquipButton.SetActive(true);
            });
        }
    }

    string DescriptionFor(InventoryItem item) {
        return item.isWeapon ? Weapon.Get(item.name).GetFullDescription() : Armour.Get(item.name).GetFullDescription();
    }

    public void EquipSelectedItem() {
        PlayerSave.current.inventory.Equip(soldier, selectedItem);
        if (selectedItem.isWeapon) DisplayWeapons();
        else DisplayArmour();
        Open(soldier);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void DisplayWeapons() {
        DisplayItems(InventoryItem.Type.Weapon);
        descriptionText.text = DescriptionFor(soldier.weapon);
        weaponEquipButton.SetActive(false);
        armourEquipButton.SetActive(false);
    }

    public void DisplayArmour() {
        DisplayItems(InventoryItem.Type.Armour);
        descriptionText.text = DescriptionFor(soldier.armour);
        weaponEquipButton.SetActive(false);
        armourEquipButton.SetActive(false);
    }
}