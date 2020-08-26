using UnityEngine;
using UnityEngine.UI;
using Data;

public class WorkshopMenu : MonoBehaviour {

    public WorkshopController workshopController;
    public GameObject scrapButton;
    public GameObject analyseButton;
    public GameObject buildButton;
    public Text infoText;
    public Transform itemsContainer;
    public Transform blueprintsContainer;
    public GameObject workshopItemPrefab;

    void Start() {
        scrapButton.SetActive(false);
        analyseButton.SetActive(false);
        buildButton.SetActive(false);
    }
    
    public void SelectInventoryItem(WorkshopItem item) {
        infoText.text = item.itemName;
        scrapButton.SetActive(true);
        analyseButton.SetActive(true);
        buildButton.SetActive(false);
    }

    public void SelectBlueprintItem(WorkshopItem item) {
        infoText.text = item.itemName;
        scrapButton.SetActive(false);
        analyseButton.SetActive(false);
        buildButton.SetActive(true);
    }

    public void DisplayItems(WorkshopState state) {
        foreach (Transform child in itemsContainer) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in blueprintsContainer) {
            Destroy(child.gameObject);
        }
        workshopController.args = new WorkshopController.Args {
            items = state.items,
            blueprints = state.blueprints
        };
        foreach (var item in state.items) {
            var transform = InstantiateItem(item);
            transform.SetParent(itemsContainer, false);
            var button = transform.GetComponent<ButtonHandler>();
            button.action.AddListener(() => workshopController.SelectInventoryItem(item.itemId));
        }
        foreach (var item in state.blueprints) {
            var transform = InstantiateItem(item);
            transform.SetParent(blueprintsContainer, false);
            var button = transform.GetComponent<ButtonHandler>();
            button.action.AddListener(() => workshopController.SelectBlueprintItem(item.itemName));
        }
    }

    Transform InstantiateItem(WorkshopItem item) {
        return Instantiate(workshopItemPrefab).transform;
    }
}