using System.Linq;
using Data;
using UnityEngine;

public class SpecialAbilityPanel : MonoBehaviour {

    public static SpecialAbilityPanel instance { get; private set; }

    public Transform abilityIconPrefab;
    public Transform buttonLayout;
    public UIController controller;

    void Start() {
        instance = this;
        Close();
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Open(ActorAction[] actions) {
        gameObject.SetActive(true);
        foreach (Transform child in buttonLayout) {
            Destroy(child.gameObject);
        }
        foreach (var action in actions.Where(ac => ac.type == ActorActionType.Special)) {
            InstantiateButton(action);
        }
    }

    void InstantiateButton(ActorAction specialAction) {
        var button = Instantiate(abilityIconPrefab) as Transform;
        button.SetParent(buttonLayout, false);
        var script = button.GetComponent<AbilityIcon>();
        script.DisplaySpriteFor(specialAction.specialAction);
        script.OnClick = () => {
            controller.DisplaySpecialAbilityTargets(specialAction.specialAction);
        };
    }
}