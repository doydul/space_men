using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class MissionCompleteController : Controller {

    public OpenArmouryInteractor openArmouryInteractor { private get; set; }

    public void ProceedToArmoury() {
        if (!disabled) {
            openArmouryInteractor.Interact(new OpenArmouryInput());
        }
    }
}
