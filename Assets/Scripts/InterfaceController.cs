using UnityEngine;

public class InterfaceController : MonoBehaviour {

    public static InterfaceController instance;

    public GameObject abilityButtonPrototype;

    void Awake() {
        instance = this;
        abilityButtonPrototype.SetActive(false);
    }

    public void EndTurn() {
        UIState.instance.EndPlayerTurn();
        GameEvents.Trigger("alien_turn_start");
    }

    public void DisplayAbilities(Ability[] abilities) {
        ClearAbilities();
        foreach (var ability in abilities) {
            var newIcon = Instantiate(abilityButtonPrototype, abilityButtonPrototype.transform.parent) as GameObject;
            newIcon.SetActive(true);
            var abilityIcon = newIcon.GetComponent<AbilityIcon>();
            abilityIcon.OnClick = () => {
                if (ability.CanUse()) ability.Use();
            };
            abilityIcon.DisplaySpriteFor(ability.name);
        }
    }

    public void ClearAbilities() {
        var parent = abilityButtonPrototype.transform.parent;
        for (int i = 1; i < parent.childCount; i++) {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}