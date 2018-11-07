using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class WorkshopMenuController : SceneMenu {

    public const string sceneName = "WorkshopMenu";

    public WorkshopInfoPanelController infoPanel;
    public List<WorkshopItemDisplayPanelController> inventoryItemPanels;
    public List<WorkshopItemDisplayPanelController> blueprintPanels;

    private ScrollableList<InventoryItem> inventoryItemScroll;
    private ScrollableList<InventoryItem> blueprintScroll;

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    public void ScrollInventoryUp() {
        inventoryItemScroll.DecreaseScroll();
        SetItemPanels();
    }

    public void ScrollInventoryDown() {
        inventoryItemScroll.IncreaseScroll();
        SetItemPanels();
    }

    public void ScrollBlueprintsUp() {
        blueprintScroll.DecreaseScroll();
        SetBlueprintPanels();
    }

    public void ScrollBlueprintsDown() {
        blueprintScroll.IncreaseScroll();
        SetBlueprintPanels();
    }

    public void UpdateInterface() {
        SetBlueprintPanels();
        SetItemPanels();
    }

    protected override void _Awake() {
        if (Squad.active == null) Squad.SetActive(Squad.GenerateDefault());

        inventoryItemScroll = new ScrollableList<InventoryItem>(Squad.items, inventoryItemPanels.Count);
        var blueprints = Squad.blueprints.Select(blueprint => blueprint.item).ToList();
        blueprintScroll = new ScrollableList<InventoryItem>(blueprints, blueprintPanels.Count);

        foreach (var panel in inventoryItemPanels) { panel.SetCallback(SelectInventoryItem); }
        foreach (var panel in blueprintPanels) { panel.SetCallback(SelectBlueprint); }

        UpdateInterface();
    }

    public void SelectBlueprint(InventoryItem blueprint) {
        infoPanel.SelectBlueprint(blueprint);
    }

    public void SelectInventoryItem(InventoryItem item) {
        infoPanel.SelectInventoryItem(item);
    }

    // Pivate

    private void SetItemPanels() {
        inventoryItemScroll.Update(Squad.items);
        for (int i = 0; i < inventoryItemPanels.Count; i++) {
            if (i >= inventoryItemScroll.GetCurrentView().Count) {
                inventoryItemPanels[i].SetItem(null);
            } else {
                inventoryItemPanels[i].SetItem(inventoryItemScroll.GetCurrentView()[i]);
            }
        }
    }

    private void SetBlueprintPanels() {
        var blueprints = Squad.blueprints.Select(blueprint => blueprint.item).ToList();
        blueprintScroll.Update(blueprints);
        for (int i = 0; i < blueprintPanels.Count; i++) {
            if (i >= blueprintScroll.GetCurrentView().Count) {
                blueprintPanels[i].SetItem(null);
            } else {
                blueprintPanels[i].SetItem(blueprintScroll.GetCurrentView()[i]);
            }
        }
    }
}
