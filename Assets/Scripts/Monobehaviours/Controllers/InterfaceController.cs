using System.Collections;
using System.Linq;
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
        AnimationManager.instance.StartAnimation(PerformEndTurn());
    }

    public void DisplayAbilities(Ability[] abilities) {
        ClearAbilities();
        foreach (var ability in abilities) {
            if (ability is StandardShoot) continue;
            var newIcon = Instantiate(abilityButtonPrototype, abilityButtonPrototype.transform.parent) as GameObject;
            newIcon.SetActive(true);
            var abilityIcon = newIcon.GetComponent<AbilityIcon>();
            abilityIcon.OnClick = () => {
                Tutorial.Show(abilityIcon.transform, "abilities1", true, true);
                Tutorial.Show("abilities2");
                if (ability.CanUse()) ability.Use();
                else {
                    AnimationManager.instance.StartAnimation(
                        NotificationPopup.PerformShow(ability.userFacingName, ability.CantUseExplanation(), new BtnData("ok", () => {}))
                    );
                }
            };
            abilityIcon.DisplaySpriteFor(ability); 
            if (!ability.CanUse()) abilityIcon.Disable();
        }
    }

    public void ClearAbilities() {
        var parent = abilityButtonPrototype.transform.parent;
        for (int i = 1; i < parent.childCount; i++) {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    
    IEnumerator PerformEndTurn() {
        if (Map.instance.GetActors<Soldier>().Where(soldier => soldier.hasActions).Any()) {
            bool ok = false;
            yield return NotificationPopup.PerformShow("end turn", "1 or more soldiers still have action points left, do you really want to end the turn?", new BtnData("cancel"), new BtnData("ok", () => ok = true));
            if (!ok) yield break;
        }
        UIState.instance.EndPlayerTurn();
        GameEvents.Trigger("alien_turn_start");
        endTurnButton.SetActive(false);
    }
}