using UnityEngine;

public class MissionOverview : MonoBehaviour {

    public BlackFade blackFade;
    public GameHUD gameHUD;
    
    public void Close() {
        blackFade.BeginFade(() => {
            gameHUD.Open();
            gameObject.SetActive(false);
        });
    }
}