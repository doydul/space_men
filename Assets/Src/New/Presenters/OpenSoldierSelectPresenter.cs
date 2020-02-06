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
                    name = "Dave"
                },
                selectables = input.selectableSoldiers.Select(soldier => new SelectionMenuInitializer.Args.Selectable {
                    index = input.index,
                    type = SelectionMenuInitializer.Args.SelectableType.Soldiers,
                    id = soldier.soldierId,
                    name = "Dave"
                }).ToArray()
            });
        });
    }
}

