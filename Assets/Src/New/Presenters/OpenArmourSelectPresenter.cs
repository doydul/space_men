using System.Linq;

using Data;

public class OpenArmourSelectPresenter : Presenter, IPresenter<OpenArmourSelectOutput> {
  
    public static OpenArmourSelectPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenArmourSelectOutput input) {
        blackFade.BeginFade(() => {
            SelectionMenuInitializer.OpenScene(new SelectionMenuInitializer.Args {
                backAction = () => {
                    InventoryInitializer.OpenScene(new InventoryInitializer.Args {
                        metaSoldierId = input.soldierId
                    });
                },
                currentSelection = new SelectionMenuInitializer.Args.Selectable {
                    type = SelectionMenuInitializer.Args.SelectableType.Armour,
                    id = input.currentArmour.itemId,
                    name = input.currentArmour.name
                },
                selectables = input.inventoryArmour.Select(item => new SelectionMenuInitializer.Args.Selectable {
                    newOwnerId = input.soldierId,
                    type = SelectionMenuInitializer.Args.SelectableType.Armour,
                    id = item.itemId,
                    name = item.name
                }).ToArray()
            });
        });
    }
}

