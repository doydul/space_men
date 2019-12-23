using Data;
using UnityEngine.SceneManagement;

public class LoadGamePresenter : Presenter, IPresenter<LoadGameOutput> {
  
    public static LoadGamePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public BlackFade blackFade;
    
    public void Present(LoadGameOutput input) {
        if (input.success) {
            blackFade.BeginFade(() => {
                SceneManager.LoadScene("MissionOverview");
            });
        }
    }
}

