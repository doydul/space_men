using Data;
using UnityEngine;

public class ShipAbilitiesPanel : MonoBehaviour {
    
    public Transform shipAbilityButtonPrefab;
    public Transform buttonLayout;
    public UIController controller;

    void Start() {
        Close();
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Open(ShipAbilityInfo[] shipAbilities) {
        gameObject.SetActive(true);
        foreach (Transform child in buttonLayout) {
            Destroy(child.gameObject);
        }
        foreach (var ability in shipAbilities) {
            InstantiateButton(ability);
        }
    }

    void InstantiateButton(ShipAbilityInfo shipAbility) {
        var button = Instantiate(shipAbilityButtonPrefab) as Transform;
        button.SetParent(buttonLayout);
        var script = button.GetComponent<ShipAbilityButton>();
        script.DisplaySpriteFor(shipAbility.type);
        script.OnClick = () => {
            Close();
            controller.DisplayShipAbilityTargets(shipAbility.type);
        };
        // TODO: grey out button if it isn't usable
    }
}