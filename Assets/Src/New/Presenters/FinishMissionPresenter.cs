using UnityEngine;
using Data;

public class FinishMissionPresenter : Presenter, IPresenter<FinishMissionOutput> {
  
    public static FinishMissionPresenter instance { get; private set; }

    public GameObject victoryPopup;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(FinishMissionOutput input) {
        victoryPopup.SetActive(true);
    }
}

