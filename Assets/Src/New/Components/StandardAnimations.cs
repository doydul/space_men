using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class StandardAnimations : MonoBehaviour {

    public SFXLayer sfxLayer;
    public Map map;
    public BloodSplatController bloodSplats;
    
    public Transform gunflarePrefab;
    public Transform hitPrefab;
    public Transform explosionCloudPrefab;
    public Transform firePrefab;
    public Transform healthBarPrefab;
    public Transform missMarkerPrefab;
    public Transform deflectMarkerPrefab;

    public void NormalShootAnimation(long soldierIndex, DamageInstance[] damageInstances, Action callback) {
        StartCoroutine(DoNormalShootAnimation(soldierIndex, damageInstances, callback));
    }

    public void ExplosiveShootAnimation(long soldierIndex, ExplosionData explosion, Action callback = null) {
        StartCoroutine(DoExplosiveShootAnimation(soldierIndex, explosion, callback));
    }

    public IEnumerator DoNormalShootAnimation(long soldierIndex, DamageInstance[] damageInstances, Action callback = null) {
        var soldier = map.GetActorByIndex(soldierIndex) as Soldier;
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(muzzleFlash);
        if (damageInstances.Length > 0) {
            var damageInstance = damageInstances[0];
            yield return DamageInstancePresenter.instance.Present(damageInstance);
        }
        if (callback != null) callback();
    }

    public IEnumerator DoExplosiveShootAnimation(long soldierIndex, ExplosionData explosion, Action callback = null) {
        var blastCoverage = explosion.squaresCovered;
        var damageInstances = explosion.damageInstances;
        var soldier = map.GetActorByIndex(soldierIndex) as Soldier;
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        var muzzleFlash = sfxLayer.SpawnPrefab(gunflarePrefab, soldier.muzzleFlashLocation.position, soldier.muzzleFlashLocation.rotation);
        yield return new WaitForSeconds(1);
        Destroy(muzzleFlash);
        var clouds = new List<GameObject>();
        foreach (var cloudPosition in blastCoverage) {
            var realPosition = map.GetTileAt(new Vector2(cloudPosition.x, cloudPosition.y)).transform.position;
            clouds.Add(sfxLayer.SpawnPrefab(explosionCloudPrefab, realPosition, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360))));
        }
        if (explosion.fires != null) {
            foreach (var fire in explosion.fires) {
                InstantiateFire(fire.position, fire.index);
            }
        }
        yield return new WaitForSeconds(1);
        foreach (var cloudObject in clouds) {
            Destroy(cloudObject);
        }
        foreach (var damageInstance in damageInstances) {
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
        if (callback != null) callback();
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

    void InstantiateFire(Position position, long index) {
        var trans = Instantiate(firePrefab) as Transform;
        var tile = map.GetTileAt(new Vector2(position.x, position.y));
        var script = trans.GetComponent<FireComponent>();
        script.index = index;
        if (tile.backgroundActor != null) {
            Destroy(tile.backgroundActor.gameObject);
        }
        tile.SetActor(trans, true);
    }
}