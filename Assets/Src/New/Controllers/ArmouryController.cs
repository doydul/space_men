using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class ArmouryController : Controller {

    public BlackFade blackFade;

    public OpenArmouryInteractor openArmouryInteractor { private get; set; }
    public OpenSoldierSelectInteractor openSoldierSelectInteractor { private get; set; }

    public void InitPage() {
        openArmouryInteractor.Interact(new OpenArmouryInput());
    }

    public void ProceedToMissionOverview() {
        if (!disabled) {
            blackFade.BeginFade(() => {
                SceneManager.LoadScene("MissionOverview");
            });
        }
    }

    public void ProceedToWorkshop() {
        if (!disabled) {
            blackFade.BeginFade(() => {
                SceneManager.LoadScene("WorkshopMenu");
            });
        }
    }

    public void GoToSelectSoldierScreen(int squadPositionIndex) {
        if (!disabled) {
            openSoldierSelectInteractor.Interact(new OpenSoldierSelectInput {
                squadPositionIndex = squadPositionIndex
            });
        }
    }

    public void GoToInventoryScreen(long metaSoldierId) {
        if (!disabled) {
            blackFade.BeginFade(() => {
                InventoryInitializer.OpenScene(new InventoryInitializer.Args {
                    metaSoldierId = metaSoldierId
                });
            });
        }
    }
}
