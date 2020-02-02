using Interactors;
using Data;

// { typeof(SelectionMenuController),
//     new Dictionary<Type, Type> {
//         { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
//     }
// }
public class SelectionMenuController : Controller {

    public SelectionMenu selectionMenu;

    public EquipItemInteractor equipItemInteractor { get; set; }

    SelectionMenuInitializer.Args.Selectable selectable;

    public void DoSomeAction() {
        if (!disabled) {
            // doSomeActionInteractor.Interact(new DoSomeActionInput());
        }
    }

    public void SelectItem(SelectionMenuInitializer.Args.Selectable selectable) {
        if (!disabled) {
            this.selectable = selectable;
            selectionMenu.DisplaySelectable(selectable);
        }
    }

    public void ConfirmSelection() {
        if (!disabled) {
            equipItemInteractor.Interact(new EquipItemInput {
                itemId = selectable.id,
                soldierId = selectable.newOwnerId
            });
        }
    }
}
