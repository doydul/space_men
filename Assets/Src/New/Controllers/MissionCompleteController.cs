using Interactors;
using Data;
using UnityEngine.SceneManagement;

public class MissionCompleteController : Controller {

    public BlackFade blackFade;

    public void ProceedToArmoury() {
        if (!disabled) {
            blackFade.BeginFade(() => {
                ArmouryInitializer.OpenScene();
            });
        }
    }
}
