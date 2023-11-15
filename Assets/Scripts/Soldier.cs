using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Soldier : Actor {

    public Armour armour;
    public Weapon weapon;
    public Transform muzzleFlashLocation;
    
    public int tilesMoved { get; set; }
    public int actionsSpent { get; set; }
    public int shotsSpent { get; set; }
    public int shotsFiredThisRound { get; set; }
    public int maxAmmo { get; set; }
    public int exp { get; set; }
    public int level { get; set; }
    public int sightRange { get; set; }

    public ReactionAbility reaction { get; set; }

    public int ammo => weapon.ammo;
    public bool canAct => actionsSpent <= 0;
    public bool canShoot => canAct && shotsSpent < ammo;
    public int shotsRemaining { get { return ammo - shotsSpent; } }
    public bool hasAmmo { get { return shotsRemaining > 0; } }
    public int baseMovement { get { return armour.movement; } }
    public int totalMovement { get { return baseMovement + armour.sprint; } }
    public int remainingMovement { get { return totalMovement - tilesMoved; } }
    public bool sprinted { get { return tilesMoved > baseMovement; } }
    public bool moved { get { return tilesMoved > 0; } }
    public int accuracy { get { return weapon.accuracy; } }
    public int range { get { return weapon.range; } }
    public int halfRange { get { return weapon.range / 2; } }
    public int minDamage { get { return weapon.minDamage; } }
    public int maxDamage { get { return weapon.maxDamage; } }
    public int shots { get { return weapon.shots; } }
    public float blast { get { return weapon.blast; } }
    public bool firesOrdnance { get { return weapon.ordnance; } }
    public Weapon.Type weaponType { get { return weapon.type; } }
    public Vector2 muzzlePosition { get { return muzzleFlashLocation.position; } }

    public bool CanSee(Vector2 gridLocation) => Map.instance.CanBeSeenFrom(new SoldierLosMask(), gridLocation, this.gridLocation);
    public bool InRange(Vector2 gridLocation) => Map.instance.ManhattanDistance(this.gridLocation, gridLocation) <= range;
    public bool InHalfRange(Vector2 gridLocation) => Map.instance.ManhattanDistance(this.gridLocation, gridLocation) <= halfRange;
    public void Shoot(Alien target) => AnimationManager.instance.StartAnimation(GameplayOperations.PerformSoldierShoot(this, target));
    public IEnumerator PerformShoot(Alien target) => GameplayOperations.PerformSoldierShoot(this, target);
    public bool HasAbility<T>() => abilities.Any(ability => ability is T);
    
    public string armourName { get { return armour.name; } }
    public string weaponName { get { return weapon.name; } }

    public List<Ability> abilities = new();

    public void ShowMuzzleFlash() {
        muzzleFlashLocation.gameObject.SetActive(true);
        var tmp = muzzleFlashLocation.localScale;
        tmp.y = -tmp.y;
        muzzleFlashLocation.localScale = tmp;
    }
    public void HideMuzzleFlash() => muzzleFlashLocation.gameObject.SetActive(false);

    void Start() {
        HideMuzzleFlash();
        GameEvents.On(this, "player_turn_start", Reset);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }

    public void GetExp(int amount) {
        exp += amount;
    }

    public void Reset() {
        tilesMoved = 0;
        actionsSpent = 0;
        reaction = null;
    }

    //
    public void StartShootingPhase() {
        GameLogicComponent.userInterface.ShowAmmoIndicators(this);
    }

    public bool WithinSightArc(Vector2 targetPosition) {
      var distance = gridLocation - targetPosition;
      if (direction == Direction.Up) {
        return distance.y < 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
      } else if (direction == Direction.Down) {
        return distance.y > 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
      } else if (direction == Direction.Left) {
        return distance.x > 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
      } else {
        return distance.x < 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
      }
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    public override void Interact(Tile tile) {
        if (!tile.open) {
            Deselect();
            return;
        }
        if (Map.instance.ManhattanDistance(gridLocation, tile.gridLocation) == 1 &&  tile.GetBackgroundActor<Door>() != null) {
            AnimationManager.instance.StartAnimation(GameplayOperations.PerformOpenDoor(this, tile));
        } else if (tile.GetActor<Actor>() == null) {
            var path = Map.instance.ShortestPath(new SoldierImpassableTerrain(), gridLocation, tile.gridLocation);
            if (path != null && path.length <= remainingMovement) {
                tilesMoved += path.length;
                AnimationManager.instance.StartAnimation(GameplayOperations.PerformActorMove(this, path));
            } else {
                Deselect();
            }
        } else {
            var alien = tile.GetActor<Alien>();
            if (alien != null) {
                if (HasAbility<StandardShoot>() && InRange(alien.gridLocation) && CanSee(alien.gridLocation) && canShoot) {
                    MapHighlighter.instance.ClearHighlights();
                    actionsSpent += 1;
                    shotsSpent += 1;
                    if (weapon.isHeavy) tilesMoved += 100;
                    Shoot(alien);
                } else {
                    Deselect();
                    if (!tile.foggy) alien.Select();
                }
            } else if (tile.GetActor<Soldier>() != null) {
                tile.GetActor<Soldier>().Select();
            }
        }
    }

    public void HighlightActions() {
        MapHighlighter.instance.ClearHighlights();
        foreach (var tile in Map.instance.iterator.Exclude(new SoldierImpassableTerrain()).RadiallyFrom(gridLocation, remainingMovement)) {
            MapHighlighter.instance.HighlightTile(tile, Color.green);
        }
        if (canAct && HasAbility<StandardShoot>()) {
            foreach (var alien in Map.instance.GetActors<Alien>()) {
                if (CanSee(alien.gridLocation) && InRange(alien.gridLocation)) {
                    MapHighlighter.instance.HighlightTile(alien.tile, Color.red);
                }
            }
        }
        foreach (var tile in Map.instance.AdjacentTiles(tile)) {
            if (tile.GetBackgroundActor<Door>() != null) MapHighlighter.instance.HighlightTile(tile, Color.yellow);
        }
    }

    public override void Select() {
        UIState.instance.SetSelectedActor(this);
        RefreshUI();
    }

    public void Deselect() {
        UIState.instance.DeselectActor();
        MapHighlighter.instance.ClearHighlights();
        InterfaceController.instance.ClearAbilities();
        AmmoGauge.instance.ClearAmmo();
        InformationPanel.instance.ClearText();
    }

    public void RefreshUI() {
        HighlightActions();
        InterfaceController.instance.DisplayAbilities(abilities.ToArray());
        AmmoGauge.instance.DisplayAmmo(ammo, shotsRemaining);
        InformationPanel.instance.SetText($"Health:   {health}/{maxHealth}\nActions: {1 - actionsSpent}/1\nWeapon: {weapon.name}\n    Accuracy: {accuracy}\n    Shots: {shots}\n    Damage: {minDamage}-{maxDamage}");
    }
}

public class SoldierImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Alien>() != null || tile.GetBackgroundActor<Door>() != null;
    }
}

public class SoldierLosMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}