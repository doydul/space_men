using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WorkshopComponent : MonoBehaviour {
    
    public TMP_Text creditsText;
    public TMP_Text infoText;
    
    public ItemListElement itemPrototype;
    
    public GameObject scrapButton;
    public GameObject researchButton;
    public GameObject constructButton;
    
    public InventoryItem activeItem { get; set; }
    public InventoryItem activeBlueprint { get; private set; }
    List<ItemListElement> itemElements;
    
    void Start() {
        itemPrototype.gameObject.SetActive(false);
        scrapButton.SetActive(false);
        researchButton.SetActive(false);
        constructButton.SetActive(false);
        DisplayInventoryItems();
        infoText.text = "";
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
            DisplayInventoryItems();
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
    
    public void DisplayInventoryItems() {
        itemPrototype.transform.parent.DestroyChildren(1);
        itemElements = new();
        
        foreach (var item in PlayerSave.current.inventory.items) {
            var itemElement = Instantiate(itemPrototype, itemPrototype.transform.parent);
            itemElement.gameObject.SetActive(true);
            var textComponent = itemElement.GetComponentInChildren<TMP_Text>();
            textComponent.text = item.name;
            var buttonComponent = itemElement.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                foreach (var itemEl in itemElements) itemEl.Deselect();
                SelectItem(item);
                itemElement.Select();
            });
            itemElement.isBlueprint = false;
            itemElements.Add(itemElement);
        }
        foreach (var blueprint in PlayerSave.current.inventory.blueprints) {
            var itemElement = Instantiate(itemPrototype, itemPrototype.transform.parent);
            itemElement.gameObject.SetActive(true);
            var textComponent = itemElement.GetComponentInChildren<TMP_Text>();
            textComponent.text = blueprint.name;
            var buttonComponent = itemElement.GetComponentInChildren<ButtonHandler>();
            buttonComponent.action.AddListener(() => {
                foreach (var itemEl in itemElements) itemEl.Deselect();
                SelectBlueprint(blueprint);
                itemElement.Select();
            });
            itemElement.isBlueprint = true;
            itemElements.Add(itemElement);
        }
        DisplayCredits();
    }
    
    public void DisplayNonInventoryItemInfo(InventoryItem item) {
        if (item == null) {
            infoText.text = "";
        } else {
            infoText.text = item.isWeapon ? Weapon.Get(item.name).GetFullDescription() : Armour.Get(item.name).GetFullDescription();
        }
        foreach (var itemEl in itemElements) itemEl.Deselect();
        scrapButton.SetActive(false);
        researchButton.SetActive(false);
        constructButton.SetActive(false);
        activeItem = null;
        activeBlueprint = null;
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
        creditsText.text = $"<sup>c</sup> {PlayerSave.current.credits}";
        if (activeItem != null) {
            scrapButton.GetComponentInChildren<TMP_Text>().text = $"scrap <sup>c</sup>{GetCost(activeItem) / 2}";
            researchButton.GetComponentInChildren<TMP_Text>().text = $"research <sup>c</sup>{GetCost(activeItem) * 2}";
        } else if (activeBlueprint != null) {
            constructButton.GetComponentInChildren<TMP_Text>().text = $"fabricate <sup>c</sup>{GetCost(activeBlueprint)}";
        }
     }
    
    int GetCost(InventoryItem item) {
        return item.isWeapon ? Weapon.Get(item.name).cost : Armour.Get(item.name).cost;
    }
}