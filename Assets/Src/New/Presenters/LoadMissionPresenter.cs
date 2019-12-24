using Data;
using UnityEngine.SceneManagement;

public class LoadMissionPresenter : Presenter, IPresenter<LoadMissionOutput> {
  
    public static LoadMissionPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public BlackFade blackFade;
    
    public void Present(LoadMissionOutput input) {
        blackFade.BeginFade(() => {
            SceneManager.LoadScene(input.missionName);
        });
    }
}

