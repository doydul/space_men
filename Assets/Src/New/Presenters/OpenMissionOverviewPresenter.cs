using Data;
using UnityEngine.UI;

public class OpenMissionOverviewPresenter : Presenter, IPresenter<OpenMissionOverviewOutput> {
  
    public static OpenMissionOverviewPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public Text briefingText;
    
    public void Present(OpenMissionOverviewOutput input) {
        briefingText.text = input.missionBriefingText;
    }
}

