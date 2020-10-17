using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;

public class ExecuteSpecialAbilityPresenter : Presenter, IPresenter<ExecuteSpecialAbilityOutput> {
  
    public static ExecuteSpecialAbilityPresenter instance { get; private set; }

    public Map map;
    public MapController mapInput;
    public AllControllers controllers;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ExecuteSpecialAbilityOutput input) {
        if (input.type == SpecialActionType.FireAtGround) {
            var soldier = map.GetActorByIndex(input.abilityOutput.soldierIndex) as Soldier;
            var soldierUI = soldier.GetComponent<SoldierUIController>();
            soldierUI.SetAmmoCount(input.abilityOutput.shotsLeft);
            soldier.ammo = input.abilityOutput.ammoLeft;
            controllers.DisableAll();
            animations.ExplosiveShootAnimation(input.abilityOutput.soldierIndex, input.abilityOutput.blastCoverage, input.abilityOutput.damageInstances, () => {
                Cleanup(input.abilityOutput.soldierIndex);
            });
        } else {
            var soldier = map.GetActorByIndex(input.abilityOutput.soldierIndex) as Soldier;
            soldier.ammo = input.abilityOutput.newAmmoCount;
            if (input.abilityOutput.remainingCrateSupplies <= 0) {
                var tile = map.GetTileAt(soldier.gridLocation);
                var crate = tile.backgroundActor.GetComponent<Actor>();
                tile.RemoveBackgroundActor();
                crate.Die();
            }
            scripting.Trigger(Scripting.Event.OnCollectAmmo);
            mapInput.DisplayActions(input.abilityOutput.soldierIndex);
        }
        // Before adding any more abilities, refactor this to use a seperate component for each ability!
        // Maybe return a different object type for each ability? its a bit stupid just having
        // one massive struct that contains all plssible output for every ability
    }

    void Cleanup(long soldierIndex) {
        controllers.EnableAll();
        mapInput.DisplayActions(soldierIndex);
    }
}

