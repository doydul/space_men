using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;

public class FireAtGroundPresenter : SpecialAbilityPresenter<FireAtGround.Output> {
  
    public static FireAtGroundPresenter instance { get; private set; }

    public Map map;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public override IEnumerator Present(FireAtGround.Output input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        soldierUI.SetAmmoCount(input.shotsLeft);
        soldier.ammo = input.ammoLeft;
        yield return animations.DoExplosiveShootAnimation(input.soldierIndex, input.explosion);
    }
}

