using UnityEngine;

public class UIState : MonoBehaviour {

    public static UIState instance { get; private set; }

    void Awake() {
        instance = this;
    }
    
    Actor selectedActor;

    public void SetSelectedActor(Actor actor) {
        selectedActor = actor;
    }

    public Actor GetSelectedActor() {
        return selectedActor;
    }

    public bool IsActorSelected() {
        return selectedActor != null;
    }

    public void DeselectActor() {
        selectedActor = null;
    }
}