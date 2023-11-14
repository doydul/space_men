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

    protected override void Awake() {
        base.Awake();
        attackIndicator.enabled = false;
        GameEvents.On(this, "alien_turn_start", Reset);
    }

    void OnDestroy() => GameEvents.RemoveListener(this, "alien_turn_start");

    public void Reset() => hasActed = false;

    public override void Select() {
        UIState.instance.SetSelectedActor(this);
        InfoPanel.instance.SetText($"Type: {type}\nHealth: {health}/{maxHealth}\nDamage: {damage}\nMovement: {movement}");
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
        InfoPanel.instance.ClearText();
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
        return !tile.open || tile.GetActor<Soldier>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}