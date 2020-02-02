using UnityEngine;
using UnityEngine.UI;

public class SelectionMenuPanel : MonoBehaviour {
    
    public Image foregroundImage;

    public SelectionMenuInitializer.Args.Selectable selectable { private get; set; }
    public SelectionMenuController controller { private get; set; }

    public void Init() {
        if (selectable.type == SelectionMenuInitializer.Args.SelectableType.Weapons) {
                
        } else if (selectable.type == SelectionMenuInitializer.Args.SelectableType.Armour) {
            // TODO
        } else {
            // TODO
        }
    }

    public void Click() {
        controller.SelectItem(selectable);
    }
}