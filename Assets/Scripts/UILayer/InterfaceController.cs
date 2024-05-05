using UnityEngine;

public class InterfaceController : MonoBehaviour {

    public static InterfaceController instance;

    public GameObject endTurnButton;
    public GameObject abilityButtonPrototype;

    void Awake() {
        instance = this;
        abilityButtonPrototype.SetActive(false);
    }

    void Start() {
        GameEvents.On(this, "player_turn_start", PlayerTurnStart);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }

    void PlayerTurnStart() {
        endTurnButton.SetActive(true);
    }

    public void EndTurn() {
        UIState.instance.EndPlayerTurn();
        GameEvents.Trigger("alien_turn_start");
        endTurnButton.SetActive(false);
    }

    public void DisplayAbilities(Ability[] abilities) {
        ClearAbilities();
        foreach (var ability in abilities) {
            if (ability is StandardShoot) continue;
            var newIcon = Instantiate(abilityButtonPrototype, abilityButtonPrototype.transform.parent) as GameObject;
            newIcon.SetActive(true);
            var abilityIcon = newIcon.GetComponent<AbilityIcon>();
            abilityIcon.OnClick = () => {
                if (ability.CanUse()) ability.Use();
            };
            abilityIcon.DisplaySpriteFor(ability.name);
            if (!ability.CanUse()) abilityIcon.Disable();
        }
    }

    public void ClearAbilities() {
        var parent = abilityButtonPrototype.transform.parent;
        for (int i = 1; i < parent.childCount; i++) {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}