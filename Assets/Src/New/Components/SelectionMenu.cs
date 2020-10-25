using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionMenu : MonoBehaviour {
    
    public SelectionMenuController selectionMenuController;
    public Transform selectionMenuPanelPrefab;
    public ScrollableSelector selector;
    public TMP_Text leftText;
    public TMP_Text rightText;

    SelectionMenuInitializer.Args args { get; set; }

    int scrollIndex;
    int width = 3;

    public void Init(SelectionMenuInitializer.Args args) {
        this.args = args;
        InitSelectableItems();
    }

    public void InitSelectableItems() {
        foreach (var selectable in args.selectables) {
            var selectionMenuPanelTransform = Instantiate(selectionMenuPanelPrefab) as Transform;
            var selectionMenuPanel = selectionMenuPanelTransform.GetComponent<SelectionMenuPanel>();
            selector.AddButton(selectionMenuPanelTransform);
            selectionMenuPanel.selectable = selectable;
            selectionMenuPanel.controller = selectionMenuController;
            selectionMenuPanel.Init();
        }
    }

    public void DisplaySelectable(SelectionMenuInitializer.Args.Selectable selectable) {
        leftText.text = selectable.name;
        rightText.text = selectable.description;
    }
}