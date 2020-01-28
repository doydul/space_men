using Interactors;
using Data;
using UnityEngine.SceneManagement;

// { typeof(ArmouryController),
//     new Dictionary<Type, Type> {
//         { typeof(DoSomeActionInteractor), typeof(DoSomeActionPresenter) }
//     }
// }
public class ArmouryController : Controller {

    public BlackFade blackFade;

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
}
