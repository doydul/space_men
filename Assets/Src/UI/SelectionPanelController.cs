using UnityEngine;
using UnityEngine.UI;

public class SelectionPanelController : MonoBehaviour {

    public SelectionMenuController menuController;
    public Image picture;

    private SelectableItem _selectableItem;
    public SelectableItem selectableItem {
        set {
            _selectableItem = value;
            ShowItemSprite();
        }
    }

    void ShowItemSprite() {
        if (_selectableItem == null) {
            picture.gameObject.SetActive(false);
        } else {
            picture.gameObject.SetActive(true);
            picture.sprite = _selectableItem.sprite;
        }
    }

    void Start() {
        ShowItemSprite();
    }

    public void SelectItem() {
        menuController.SelectItem(_selectableItem);
    }
}
