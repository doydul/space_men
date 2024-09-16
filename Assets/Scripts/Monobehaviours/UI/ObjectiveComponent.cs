using UnityEngine;
using TMPro;

public class ObjectiveComponent : MonoBehaviour {
    
    public Toggle checkbox;
    public TMP_Text label;
    
    public Objective objective { get; private set; }
    
    void Start() {
        GameEvents.On(this, "alien_turn_start", SetCheckboxState);
        SetCheckboxState();
    }
    
    void OnDestroy() {
        GameEvents.RemoveListener(this, "alien_turn_start");
    }
    
    public void SetCheckboxState() {
        if (objective != null) checkbox.SetState(objective.complete);
        else checkbox.SetState(false);
    }
    
    public void SetObjective(Objective objective) {
        this.objective = objective;
        label.text = objective.description;
        objective.ui = this;
    }
    
    public void Zoom() {
        CameraController.CentreCameraOn(objective.targetLocation);
    }
}