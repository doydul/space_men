using UnityEngine;

public class UIState : MonoBehaviour {

    public static UIState instance { get; private set; }

    void Awake() {
        instance = this;
        playerTurn = true;
    }
    
    Actor selectedActor;
    bool playerTurn;

    public void SetSelectedActor(Actor actor) => selectedActor = actor;
    public Actor GetSelectedActor() => selectedActor;
    public bool IsActorSelected() => selectedActor != null;
    public void DeselectActor() => selectedActor = null;
    public void EndPlayerTurn() => playerTurn = false;
    public void StartPlayerTurn() => playerTurn = true;
    public bool IsPlayerTurn() => playerTurn;
    public bool IsAlienTurn() => !playerTurn;
}