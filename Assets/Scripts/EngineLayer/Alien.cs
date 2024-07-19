using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Alien : Actor {

    public class Pod {
        public List<Alien> members = new();
    }

    public string type { get; set; }
    public int armour { get; set; }
    public int accModifier { get; set; }
    public int damage { get; set; }
    public int armourPen { get; set; }
    public int movement { get; set; }
    public float threat { get; set; }
    public int expReward { get; set; }
    public int sensoryRange { get; set; }
    public AlienAudioProfile audio { get; set; }
    public Pod pod { get; set; }

    public bool hasActed;
    public bool awake;

    public SpriteRenderer attackIndicator;

    public bool canAct => !hasActed && awake;
    public void ShowAttack() => attackIndicator.enabled = true;
    public void HideAttack() => attackIndicator.enabled = false;

    protected override void Awake() {
        base.Awake();
        attackIndicator.enabled = false;
        GameEvents.On(this, "alien_turn_start", Reset);
    }

    void OnDestroy() => GameEvents.RemoveListener(this, "alien_turn_start");

    public void Reset() => hasActed = false;

    public void Awaken() {
        if (!awake) {
            awake = true;
            if (pod != null) {
                foreach (var alien in pod.members) alien.awake = true;
            }
        }
    }

    public override bool Hurt(int damage, DamageType damageType = DamageType.Normal) {
        int effectiveArmour = damageType == DamageType.Energy ? (int)Mathf.Round(armour / 4f) : armour;
        if (damage <= effectiveArmour / 2) {
            base.Hurt((int)Mathf.Round(damage / 4f));
        } else if (damage <= effectiveArmour) {
            base.Hurt((int)Mathf.Round(damage / 2f));
        } else {
            PlayAudio(audio.hurt.Sample());
            base.Hurt(damage);
        }
        Awaken();
        return damage > effectiveArmour;
    }

    public override void Select() {
        UIState.instance.SetSelectedActor(this);
        InformationPanel.instance.SetText($"Type: {type}\nHealth: {health}/{maxHealth}\nArmour: {armour}\nAccuracy Modifier: {accModifier}\nDamage: {damage}\nMovement: {movement}");
        HighlightActions();
    }

    public override void Interact(Tile tile) {
        var actor = tile.GetActor<Actor>();
        Deselect();
        if (actor != null) {
            actor.Select();
        }
    }

    public void Deselect() {
        UIState.instance.DeselectActor();
        MapHighlighter.instance.ClearHighlights();
        InformationPanel.instance.ClearText();
    }

    public void HighlightActions() {
        MapHighlighter.instance.ClearHighlights();
        foreach (var tile in Map.instance.iterator.Exclude(new AlienImpassableTerrain()).RadiallyFrom(gridLocation, movement)) {
            MapHighlighter.instance.HighlightTile(tile, Color.red);
            foreach (var adjTile in Map.instance.AdjacentTiles(tile)) {
                if (adjTile.GetActor<Soldier>() != null) {
                    MapHighlighter.instance.HighlightTile(adjTile, Color.red);
                }
            }
        }
    }
}

public class AlienImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Soldier>() != null || tile.GetBackgroundActor<Door>() != null || tile.GetBackgroundActor<Chest>() != null;
    }
}