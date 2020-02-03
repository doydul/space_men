using System.Linq;
using Data;

public class OpenWeaponSelectPresenter : Presenter, IPresenter<OpenWeaponSelectOutput> {
  
    public static OpenWeaponSelectPresenter instance { get; private set; }

    public BlackFade blackFade;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenWeaponSelectOutput input) {
        blackFade.BeginFade(() => {
            SelectionMenuInitializer.OpenScene(new SelectionMenuInitializer.Args {
                backAction = () => {
                    InventoryInitializer.OpenScene(new InventoryInitializer.Args {
                        metaSoldierId = input.soldierId
                    });
                },
                currentSelection = new SelectionMenuInitializer.Args.Selectable {
                    type = SelectionMenuInitializer.Args.SelectableType.Weapons,
                    id = input.currentWeapon.itemId,
                    name = input.currentWeapon.name
                },
                selectables = input.inventoryWeapons.Select(item => new SelectionMenuInitializer.Args.Selectable {
                    newOwnerId = input.soldierId,
                    type = SelectionMenuInitializer.Args.SelectableType.Weapons,
                    id = item.itemId,
                    name = item.name
                }).ToArray()
            });
        });
    }
}

