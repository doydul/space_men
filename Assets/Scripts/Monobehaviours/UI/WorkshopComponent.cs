using UnityEngine;
using TMPro;

public class WorkshopComponent : MonoBehaviour {
    
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
        Open();
    }
    
    public void Open() {
        DisplayInventoryItems();
        DisplayBlueprints();
    }
    
    public void Close() {
        
    }
    
    public void SelectItem(InventoryItem item) {
        activeBlueprint = null;
        activeItem = item;
        scrapButton.SetActive(true);
        researchButton.SetActive(true);
        constructButton.SetActive(false);
    }
    
    public void SelectBlueprint(InventoryItem blueprint) {
        activeItem = null;
        activeBlueprint = blueprint;
        scrapButton.SetActive(false);
        researchButton.SetActive(false);
        constructButton.SetActive(true);
    }
    
    public void ScrapItem(InventoryItem item) {
        
    }
    
    public void ResearchItem(InventoryItem item) {
        
    }
    
    public void ConstructItem(InventoryItem blueprint) {
        
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
    }
}