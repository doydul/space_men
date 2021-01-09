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
                    name = item.name,
                    description = GenerateWeaponDescription(item.weaponStats)
                }).ToArray()
            });
        });
    }

    string GenerateWeaponDescription(WeaponStats weaponStats) {
        return "accuracy: " + weaponStats.accuracy + "\n" +
               "armour penetration %: " + weaponStats.armourPen + "\n" +
               "damage: " + weaponStats.minDamage + "-" + weaponStats.maxDamage + "\n" +
               "shots after moving: " + weaponStats.shotsWhenMoving + "\n" +
               "shots when stationary: " + weaponStats.shotsWhenStill + "\n" +
               (weaponStats.blast > 0 ? "blast: " + weaponStats.blast : "") +
               "ammo capacity: " + weaponStats.ammo + "\n" +
               "value: " + weaponStats.cost;
    }
}

