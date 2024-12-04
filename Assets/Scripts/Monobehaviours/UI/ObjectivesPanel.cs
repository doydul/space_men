using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectivesPanel : MonoBehaviour {
    
    public static ObjectivesPanel instance;
    
    public Transform primaryObjectivePrototype;
    public Transform secondaryObjectivePrototype;
    public GameObject showObjectiesButton;
    
    void Awake() => instance = this;
    
    void Start() {
        primaryObjectivePrototype.gameObject.SetActive(false);
        secondaryObjectivePrototype.gameObject.SetActive(false);
    }
    
    void DisplayObjectives(Transform prototype, List<Objective> objectives) {
        prototype.parent.DestroyChildren(startIndex: 1);
        foreach (var objective in objectives) {
            var trans = Instantiate(prototype, prototype.parent);
            trans.gameObject.SetActive(true);
            var objectiveComp = trans.GetComponent<ObjectiveComponent>();
            objectiveComp.SetObjective(objective);
        }
    }
    
    public void Show() {
        showObjectiesButton.SetActive(false);
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(primaryObjectivePrototype.parent.parent as RectTransform);
    }
    
    public void Hide() {
        showObjectiesButton.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void DisplayPrimaryObjectives(List<Objective> objectives) => DisplayObjectives(primaryObjectivePrototype, objectives);
    public void DisplaySecondaryObjectives(List<Objective> objectives) => DisplayObjectives(secondaryObjectivePrototype, objectives);
}