using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;

public class SoldierShootPresenter : Presenter, IPresenter<SoldierShootOutput> {
  
    public static SoldierShootPresenter instance { get; private set; }

    public SFXLayer sfxLayer;
    public Map map;
    public MapController mapInput;
    public UIController uiInput;

    public Transform gunflarePrefab;
    public Transform hitPrefab;
    public Transform explosionCloudPrefab;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(SoldierShootOutput input) {
        StartCoroutine(Animation(input));
    }

    IEnumerator Animation(SoldierShootOutput input) {
        DisableAllInput();
        if (input.blastCoverage == null) {
            yield return StandardShootAnimation(input);
        } else {
            yield return ExplosiveShootAnimation(input);
        }
        EnableAllInput();
        mapInput.DisplayActions(input.soldierIndex);
    }

    IEnumerator StandardShootAnimation(SoldierShootOutput input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var damageInstance = input.damageInstances[0];
        var alien = map.GetActorByIndex(damageInstance.victimIndex) as Alien;
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        yield return new WaitForSeconds(1);
        Destroy(muzzleFlash);
        if (damageInstance.attackResult == AttackResult.Hit || damageInstance.attackResult == AttackResult.Killed) {
            var hitSFX = sfxLayer.SpawnPrefab(hitPrefab, alien.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            yield return new WaitForSeconds(1);
            Destroy(hitSFX);
        }
        if (damageInstance.attackResult == AttackResult.Killed) {
            alien.Die();
        }
    }

    IEnumerator ExplosiveShootAnimation(SoldierShootOutput input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        yield return new WaitForSeconds(1);
        Destroy(muzzleFlash);
        var clouds = new List<GameObject>();
        foreach (var cloudPosition in input.blastCoverage) {
            var realPosition = map.GetTileAt(new Vector2(cloudPosition.x, cloudPosition.y)).transform.position;
            clouds.Add(sfxLayer.SpawnPrefab(explosionCloudPrefab, realPosition, Quaternion.Euler(0, 0, Random.Range(0, 360))));
        }
        yield return new WaitForSeconds(1);
        foreach (var cloudObject in clouds) {
            Destroy(cloudObject);
        }
        foreach (var damageInstance in input.damageInstances) {
            var alien = map.GetActorByIndex(damageInstance.victimIndex);
            // Show health bar i guess?
            if (damageInstance.attackResult == AttackResult.Killed) alien.Die();
        }
    }

    void DisableAllInput() {
        mapInput.Disable();
        uiInput.Disable();
    }

    void EnableAllInput() {
        mapInput.Enable();
        uiInput.Enable();
    }
}
