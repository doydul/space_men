using UnityEngine;
using UnityEngine.UI;

public class SelectionPanelController : MonoBehaviour {

    public SelectionMenuController menuController;
    public Image picture;

    private SelectableItem _selectableItem;
    public SelectableItem selectableItem {
        set {
            _selectableItem = value;
        }
    }

    void Start() {
        if (_selectableItem == null) {
            picture.gameObject.SetActive(false);
        } else {
            picture.sprite = _selectableItem.sprite;
        }
    }

    public void SelectItem() {
        menuController.SelectItem(_selectableItem);
    }
}
