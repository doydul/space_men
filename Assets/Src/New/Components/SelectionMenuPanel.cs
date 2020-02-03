using UnityEngine;
using UnityEngine.UI;

public class SelectionMenuPanel : MonoBehaviour {
    
    public Icon icon;

    public SelectionMenuInitializer.Args.Selectable selectable { private get; set; }
    public SelectionMenuController controller { private get; set; }

    public void Init() {
        if (selectable.type == SelectionMenuInitializer.Args.SelectableType.Weapons) {
            icon.Init(Weapon.Get(selectable.name));
        } else if (selectable.type == SelectionMenuInitializer.Args.SelectableType.Armour) {
            icon.Init(Armour.Get(selectable.name));
        } else {
            // TODO
        }
    }

    public void Click() {
        controller.SelectItem(selectable);
    }
}