using UnityEngine;
using TMPro;

public class WorkshopComponent : MonoBehaviour {
    
    public TMP_Text creditsText;
    public TMP_Text infoText;
    
    public Transform itemPrototype;
    public Transform blueprintPrototype;
    
    public GameObject scrapButton;
    public GameObject researchButton;
    public GameObject constructButton;
    
    InventoryItem activeItem;
    InventoryItem activeBlueprint;
    
    void Start() {
        itemPrototype.gameObject.SetActive(false);
        blueprintPrototype.gameObject.SetActive(false);
        scrapButton.SetActive(false);
        researchButton.SetActive(false);
        constructButton.SetActive(false);
        Close();
    }
    
    public void Open() {
        DisplayInventoryItems();
        DisplayBlueprints();
        gameObject.SetActive(true);
        infoText.text = "";
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void SelectItem(InventoryItem item) {
        activeBlueprint = null;
        activeItem = item;
        DisplayButtons();
    }
    
    public void SelectBlueprint(InventoryItem blueprint) {
        activeItem = null;
        activeBlueprint = blueprint;
        DisplayButtons();
    }
    
    public void ScrapItem() {
        PlayerSave.current.inventory.items.Remove(activeItem);
        PlayerSave.current.credits += GetCost(activeItem) / 2;
        activeItem = null;
        DisplayInventoryItems();
        DisplayButtons();
    }
    
    public void ResearchItem() {
        int cost = GetCost(activeItem) * 2;
        if (PlayerSave.current.credits >= cost) {
            PlayerSave.current.credits -= cost;
            PlayerSave.current.inventory.blueprints.Add(activeItem.Dup());
            DisplayBlueprints();
        }
    }
    
    public void ConstructItem() {
        int cost = GetCost(activeBlueprint);
        if (PlayerSave.current.credits >= cost) {
            PlayerSave.current.credits -= cost;
            PlayerSave.current.inventory.items.Add(activeBlueprint.Dup());
            DisplayInventoryItems();
        }
    }
    
    void DisplayBlueprints() {
        blueprintPrototype.parent.DestroyChildren(1);
        
        foreach (var blueprint in PlayerSave.current.inventory.blueprints) {
            var blueprintTrans = Instantiate(blueprintPrototype, blueprintPrototype.parent);
            blueprintTrans.gameObject.SetActive(true);
            var textComponent = blueprintTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = blueprint.name;
            var buttonComponent = blueprintTrans.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                SelectBlueprint(blueprint);
            });
        }
        DisplayCredits();
    }
    
    void DisplayInventoryItems() {
        itemPrototype.parent.DestroyChildren(1);
        
        foreach (var item in PlayerSave.current.inventory.items) {
            var itemTrans = Instantiate(itemPrototype, itemPrototype.parent);
            itemTrans.gameObject.SetActive(true);
            var textComponent = itemTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = item.name;
            var buttonComponent = itemTrans.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                SelectItem(item);
            });
        }
        DisplayCredits();
    }
    
    void DisplayButtons() {
        if (activeItem != null) {
            scrapButton.SetActive(true);
            researchButton.SetActive(true);
            constructButton.SetActive(false);
            infoText.text = activeItem.isWeapon ? Weapon.Get(activeItem.name).GetFullDescription() : Armour.Get(activeItem.name).GetFullDescription();
        } else if (activeBlueprint != null) {
            scrapButton.SetActive(false);
            researchButton.SetActive(false);
            constructButton.SetActive(true);
            infoText.text = activeBlueprint.isWeapon ? Weapon.Get(activeBlueprint.name).GetFullDescription() : Armour.Get(activeBlueprint.name).GetFullDescription();
        } else {
            scrapButton.SetActive(false);
            researchButton.SetActive(false);
            constructButton.SetActive(false);
        }
        DisplayCredits();
    }
    
    void DisplayCredits() {
        creditsText.text = $"credits {PlayerSave.current.credits}";
        if (activeItem != null) {
            scrapButton.GetComponentInChildren<TMP_Text>().text = $"scrap {GetCost(activeItem) / 2}";
            researchButton.GetComponentInChildren<TMP_Text>().text = $"research {GetCost(activeItem) * 2}";
        } else if (activeBlueprint != null) {
            constructButton.GetComponentInChildren<TMP_Text>().text = $"fabricate {GetCost(activeBlueprint)}";
        }
     }
    
    int GetCost(InventoryItem item) {
        return item.isWeapon ? Weapon.Get(item.name).cost : Armour.Get(item.name).cost;
    }
}