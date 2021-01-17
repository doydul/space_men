using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;

public class StunShotPresenter : SpecialAbilityPresenter<StunShot.Output> {
  
    public static StunShotPresenter instance { get; private set; }

    public Transform genericMarkerPrefab;
    public SFXLayer sfxLayer;
    public Map map;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public override IEnumerator Present(StunShot.Output input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var alien = map.GetActorByIndex(input.targetIndex) as Alien;

        yield return animations.DoNormalShootAnimation(input.soldierIndex, new DamageInstance[0]);
        var hitSFX = sfxLayer.SpawnPrefab(genericMarkerPrefab, alien.realLocation);
        hitSFX.GetComponent<GenericMarker>().SetText("Stunned");
        yield return new WaitForSeconds(1f);
        Destroy(hitSFX);
    }
}

