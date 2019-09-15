using Data;
using UnityEngine;

public class ProgressGamePhasePresenter : Presenter, IPresenter<ProgressGamePhaseOutput> {
  
    public static ProgressGamePhasePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ProgressGamePhaseOutput input) {
        UnityEngine.Debug.Log("GAME PHASE PROGRESSED");
        UnityEngine.Debug.Log("Phase " + input.currentPhase);
        if (input.newAliens != null) { 
            foreach (var alien in input.newAliens) {
                UnityEngine.Debug.Log("New Alien:");
                UnityEngine.Debug.Log(alien.alienType);
            }
        }
        if (input.alienActions != null) {
            UnityEngine.Debug.Log("There are some alien actions");
        }
        UnityEngine.Debug.Log("--------------------");
    }
}
