using Interactors;
using Data;

public class SelectionMenuController : Controller {

    public SelectionMenu selectionMenu;

    public EquipItemInteractor equipItemInteractor { private get; set; }
    public AddSoldierToSquadInteractor addSoldierToSquadInteractor { private get; set; }
    public SelectionMenuCancelInteractor selectionMenuCancelInteractor { private get; set; }

    SelectionMenuInitializer.Args.Selectable selectable;

    public void SelectItem(SelectionMenuInitializer.Args.Selectable selectable) {
        if (!disabled) {
            this.selectable = selectable;
            selectionMenu.DisplaySelectable(selectable);
        }
    }

    public void ConfirmSelection() {
        if (!disabled) {
            if (selectable.type == SelectionMenuInitializer.Args.SelectableType.Soldiers) {
                addSoldierToSquadInteractor.Interact(new AddSoldierToSquadInput {
                    index = selectable.index,
                    soldierId = selectable.id
                });
            } else {
                equipItemInteractor.Interact(new EquipItemInput {
                    itemId = selectable.id,
                    soldierId = selectable.newOwnerId
                });
            }
        }
    }

    public void GoBack() {
        if (!disabled) {
            selectionMenuCancelInteractor.Interact(new SelectionMenuCancelInput());
        }
    }
}
