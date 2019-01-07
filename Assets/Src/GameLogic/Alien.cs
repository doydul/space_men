using UnityEngine;
using System.Collections;

public class Alien : Actor {

    public int maxHealth;
    private int health;
    public int armour;
    public int accModifier;
    public int damage;
    public int armourPen;
    public int movement;
    public float threat;
    public int expReward;

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

    public void Hurt(int damage) {
        health -= damage;
        SetHealthIndicatorSize();
        if (dead) {
            Die(1);
        }
    }

    public void ShowHitIndicator() {
        hitIndicator.SetActive(true);
        healthIndicator.enabled = true;
        DisableIndicators();
    }

    public void ShowDeflectIndicator() {
        deflectIndicator.SetActive(true);
        DisableIndicators();
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

    public void ShowAttackIndicator() {
        attackIndicator.enabled = true;
        new Delayer(this).Wait(0.5f, () => {
            attackIndicator.enabled = false;
        });
    }
}
