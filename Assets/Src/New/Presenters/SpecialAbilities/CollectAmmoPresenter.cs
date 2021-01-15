using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;

public class CollectAmmoPresenter : SpecialAbilityPresenter<CollectAmmo.Output> {
  
    public static CollectAmmoPresenter instance { get; private set; }

    public Map map;
    public MapController mapInput;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public override IEnumerator Present(CollectAmmo.Output input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        soldier.ammo = input.newAmmoCount;
        if (input.remainingCrateSupplies <= 0) {
            var tile = map.GetTileAt(soldier.gridLocation);
            var crate = tile.backgroundActor.GetComponent<Actor>();
            tile.RemoveBackgroundActor();
            crate.Die();
        }
        scripting.Trigger(Scripting.Event.OnCollectAmmo);
        mapInput.DisplayActions(input.soldierIndex);
        yield break;
    }
}

