using UnityEngine;
using System.Collections;

public class Alien : Actor {

    public int maxHealth { get; set; }
    private int health { get; set; }
    public int armour { get; set; }
    public int accModifier { get; set; }
    public int damage { get; set; }
    public int armourPen { get; set; }
    public int movement { get; set; }
    public float threat { get; set; }
    public int expReward { get; set; }

    public GameObject hitIndicator;
    public GameObject deflectIndicator;
    public SpriteRenderer healthIndicator;
    public SpriteRenderer attackIndicator;

    private Coroutine disableIndicatorsRoutine;
    private float healthSpriteSize;

    public bool dead { get { return health <= 0; } }

    void Awake() {
        health = maxHealth;
        healthSpriteSize = healthIndicator.size.x;
        hitIndicator.SetActive(false);
        deflectIndicator.SetActive(false);
        healthIndicator.enabled = false;
        attackIndicator.enabled = false;
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

    public void Hurt(int damage) {
        health -= damage;
        SetHealthIndicatorSize();
        if (dead) {
            Die(0.2f);
        }
    }

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
