using UnityEngine;
using System.Collections;

public class Alien : Actor {

    public string type { get; set; }
    public int armour { get; set; }
    public int accModifier { get; set; }
    public int damage { get; set; }
    public int armourPen { get; set; }
    public int movement { get; set; }
    public float threat { get; set; }
    public int expReward { get; set; }
    public int sensoryRange { get; set; }

    public bool hasActed;
    public bool awake;

    public GameObject hitIndicator;
    public GameObject deflectIndicator;
    public SpriteRenderer healthIndicator;
    public SpriteRenderer attackIndicator;

    private Coroutine disableIndicatorsRoutine;
    private float healthSpriteSize;

    public bool dead { get { return health <= 0; } }
    public bool canAct => !hasActed && awake;
    public void Awaken() => awake = true;
    public void ShowAttack() => attackIndicator.enabled = true;
    public void HideAttack() => attackIndicator.enabled = false;
    public void ShowHealth() => healthIndicator.enabled = true;
    public void HideHealth() => healthIndicator.enabled = false;
    public void ShowHit() => hitIndicator.SetActive(true);
    public void HideHit() => hitIndicator.SetActive(false);

    void Awake() {
        health = maxHealth;
        healthSpriteSize = healthIndicator.size.x;
        hitIndicator.SetActive(false);
        deflectIndicator.SetActive(false);
        healthIndicator.enabled = false;
        attackIndicator.enabled = false;
        awake = true;
        GameEvents.On(this, "alien_turn_start", Reset);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "alien_turn_start");
    }

    public void Reset() {
        hasActed = false;
    }

    public void FromData(AlienData data) {
        maxHealth = data.maxHealth;
        health = data.maxHealth;
        armour = data.armour;
        accModifier = data.accModifier;
        damage = data.damage;
        armourPen = data.armourPen;
        movement = data.movement;
        threat = data.threat;
        expReward = data.expReward;
    }

    // public void Hurt(int damage) {
    //     health -= damage;
    //     SetHealthIndicatorSize();
    //     if (dead) {
    //         Die(0.2f);
    //     }
    // }

    private void SetHealthIndicatorSize() {
        var currentSize = healthIndicator.size;
        currentSize.x = health * healthSpriteSize / maxHealth;
        healthIndicator.size = currentSize;
    }

    void DisableIndicators() {
        if (disableIndicatorsRoutine != null) StopCoroutine(disableIndicatorsRoutine);
        disableIndicatorsRoutine = StartCoroutine(DisableIndicatorsRoutine());
    }

    IEnumerator DisableIndicatorsRoutine() {
        yield return new WaitForSeconds(1);
        hitIndicator.SetActive(false);
        deflectIndicator.SetActive(false);
        healthIndicator.enabled = false;
    }
}

public class AlienImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Soldier>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}