using System.Linq;
using Data;

public class OpenSoldierSelectPresenter : Presenter, IPresenter<OpenSoldierSelectOutput> {
  
    public static OpenSoldierSelectPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public BlackFade blackFade;
    
    public void Present(OpenSoldierSelectOutput input) {
        blackFade.BeginFade(() => {
            SelectionMenuInitializer.OpenScene(new SelectionMenuInitializer.Args {
                backAction = () => {
                    ArmouryInitializer.OpenScene();
                },
                currentSelection = new SelectionMenuInitializer.Args.Selectable {
                    index = input.index,
                    type = SelectionMenuInitializer.Args.SelectableType.Soldiers,
                    id = input.currentSoldier.soldierId,
                    name = input.currentSoldier.name,
                    description = DescriptionFor(input.currentSoldier)
                },
                selectables = input.selectableSoldiers.Select(soldier => new SelectionMenuInitializer.Args.Selectable {
                    index = input.index,
                    type = SelectionMenuInitializer.Args.SelectableType.Soldiers,
                    id = soldier.soldierId,
                    name = soldier.name,
                    description = DescriptionFor(soldier)
                }).ToArray()
            });
        });
    }

    string DescriptionFor(SoldierDisplayInfo soldierInfo) {
        if (soldierInfo == null || soldierInfo.empty) return "";
        return "weapon: " + soldierInfo.weaponName + "\n"
             + "armour: " + soldierInfo.armourName + "\n"
             + "exp: " + soldierInfo.exp;
    }
}

