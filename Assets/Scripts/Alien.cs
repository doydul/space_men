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

    public SpriteRenderer attackIndicator;

    public bool canAct => !hasActed && awake;
    public void Awaken() => awake = true;
    public void ShowAttack() => attackIndicator.enabled = true;
    public void HideAttack() => attackIndicator.enabled = false;

    void Start() {
        attackIndicator.enabled = false;
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
}

public class AlienImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Soldier>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}