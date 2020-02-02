using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class ArmouryController : Controller {

    public BlackFade blackFade;

    public OpenInventoryInteractor openInventoryInteractor { get; set; }

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

    public void GoToSelectSoldierScreen(long metaSoldierId) {
        if (!disabled) {
            blackFade.BeginFade(() => {
                SceneManager.LoadScene("SelectionMenu");
            });
        }
    }

    public void GoToInventoryScreen(long metaSoldierId) {
        if (!disabled) {
            openInventoryInteractor.Interact(new OpenInventoryInput {
                metaSoldierId = metaSoldierId
            });
        }
    }
}
