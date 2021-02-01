using System.Linq;
using System.Collections;
using Data;
using UnityEngine;

public class DamageInstancePresenter : Presenter {

    public static DamageInstancePresenter instance { get; private set; }

    public SFXLayer sfxLayer;
    public Map map;
    public BloodSplatController bloodSplats;
    
    public Transform gunflarePrefab;
    public Transform hitPrefab;
    public Transform healthBarPrefab;
    public Transform missMarkerPrefab;
    public Transform deflectMarkerPrefab;
    
    void Awake() {
        instance = this;
    }
    
    public IEnumerator Present(DamageInstance damageInstance) {
        Actor victim = null;
        try {
            victim = map.GetActorByIndex(damageInstance.victimIndex);
        } catch (System.Exception) {
            yield break;
        }
        if (damageInstance.attackResult == AttackResult.Hit || damageInstance.attackResult == AttackResult.Killed) {
            bloodSplats.MakeSplat(victim);
            var hitSFX = sfxLayer.SpawnPrefab(hitPrefab, victim.transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            yield return new WaitForSeconds(0.5f);
            Destroy(hitSFX);
        }
        if (damageInstance.attackResult == AttackResult.Hit) {
            victim.health = damageInstance.victimHealthAfterDamage;
            yield return HealthBarAnimation(victim.transform.position, victim.healthPercentage);
        } else if (damageInstance.attackResult == AttackResult.Killed) {
            victim.Die();
        } else {
            yield return MarkerAnimationFor(damageInstance);
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
}

