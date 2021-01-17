using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;

public class GrenadePresenter : SpecialAbilityPresenter<Grenade.Output> {
  
    public static GrenadePresenter instance { get; private set; }

    public Map map;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public override IEnumerator Present(Grenade.Output input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        yield return animations.DoExplosiveShootAnimation(input.soldierIndex, input.explosion);
    }
}

