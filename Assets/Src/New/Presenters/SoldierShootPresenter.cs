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
    public BloodSplatController bloodSplats;

    public Transform gunflarePrefab;
    public Transform hitPrefab;
    public Transform explosionCloudPrefab;
    public Transform healthBarPrefab;
    public Transform missMarkerPrefab;
    public Transform deflectMarkerPrefab;
    
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
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        var damageInstance = input.damageInstances[0];
        var alien = map.GetActorByIndex(damageInstance.victimIndex) as Alien;
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        soldierUI.SetAmmoCount(input.shotsLeft);
        yield return new WaitForSeconds(1);
        Destroy(muzzleFlash);
        if (damageInstance.attackResult == AttackResult.Hit || damageInstance.attackResult == AttackResult.Killed) {
            bloodSplats.MakeSplat(alien);
            var hitSFX = sfxLayer.SpawnPrefab(hitPrefab, alien.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            yield return new WaitForSeconds(1);
            Destroy(hitSFX);
        }
        if (damageInstance.attackResult == AttackResult.Hit) {
            alien.health = damageInstance.victimHealthAfterDamage;
            yield return HealthBarAnimation(alien.transform.position, alien.healthPercentage);
        } else if (damageInstance.attackResult == AttackResult.Killed) {
            alien.Die();
        } else {
            yield return MarkerAnimationFor(damageInstance);
        }
    }

    IEnumerator ExplosiveShootAnimation(SoldierShootOutput input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        soldierUI.SetAmmoCount(input.shotsLeft);
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
            var actor = map.GetActorByIndex(damageInstance.victimIndex);
            if (damageInstance.attackResult == AttackResult.Killed) {
                bloodSplats.MakeSplat(actor);
                actor.Die();
            } else if (damageInstance.attackResult == AttackResult.Hit) {
                bloodSplats.MakeSplat(actor);
                actor.health = damageInstance.victimHealthAfterDamage;
                yield return HealthBarAnimation(actor.transform.position, actor.healthPercentage);
            } else {
                yield return MarkerAnimationFor(damageInstance);
            }
        }
    }

    IEnumerator HealthBarAnimation(Vector2 position, int percentage) {
        var healthBarGO = sfxLayer.SpawnPrefab(healthBarPrefab, position);
        var healthBar = healthBarGO.GetComponent<HealthBar>();
        healthBar.SetPercentage(percentage);
        yield return new WaitForSeconds(1);
        Destroy(healthBarGO);
    }

    IEnumerator MarkerAnimationFor(DamageInstance damageInstance) {
        if (damageInstance.attackResult != AttackResult.Missed && damageInstance.attackResult != AttackResult.Deflected) yield break;
        var actor = map.GetActorByIndex(damageInstance.victimIndex);
        var prefab = damageInstance.attackResult == AttackResult.Missed ? missMarkerPrefab : deflectMarkerPrefab;
        var marker =  sfxLayer.SpawnPrefab(prefab, actor.realLocation);
        yield return new WaitForSeconds(1);
        Destroy(marker);
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

