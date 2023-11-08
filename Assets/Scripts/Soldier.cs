using UnityEngine;
using System.Collections;

public class Soldier : Actor {

    public Armour armour;
    public Weapon weapon;
    public Transform muzzleFlashLocation;
    
    public int tilesMoved { get; set; }
    public int actionsSpent { get; set; }
    public int shotsFiredThisRound { get; set; }
    public int ammo { get; set; }
    public int maxAmmo { get; set; }
    public int exp { get; set; }
    public int level { get; set; }

    public int totalShots { get {
        if (sprinted) {
            return 0;
        } else if (moved) {
            return weapon.shotsWhenMoving;
        } else {
            return weapon.shotsWhenStill;
        }
    } }
    public bool canAct => actionsSpent <= 0;
    public int shotsRemaining { get { return totalShots - shotsFiredThisRound; } }
    public bool hasAmmo { get { return shotsRemaining > 0; } }
    public int baseMovement { get { return armour.movement; } }
    public int totalMovement { get { return baseMovement + armour.sprint; } }
    public int remainingMovement { get { return totalMovement - tilesMoved; } }
    public bool sprinted { get { return tilesMoved > baseMovement; } }
    public bool moved { get { return tilesMoved > 0; } }
    public int accuracy { get { return weapon.accuracy; } }
    public int armourPen { get { return weapon.armourPen; } }
    public int minDamage { get { return weapon.minDamage; } }
    public int maxDamage { get { return weapon.maxDamage; } }
    public int shots { get { return weapon.shots; } }
    public float blast { get { return weapon.blast; } }
    public bool firesOrdnance { get { return weapon.ordnance; } }
    public Weapon.Type weaponType { get { return weapon.type; } }
    public Vector2 muzzlePosition { get { return muzzleFlashLocation.position; } }

    public void Shoot(Alien target) => AnimationManager.instance.StartAnimation(GameplayOperations.PerformSoldierShoot(this, target));
    
    public string armourName { get { return armour.name; } }
    public string weaponName { get { return weapon.name; } }

    public void ShowMuzzleFlash() {
        muzzleFlashLocation.gameObject.SetActive(true);
        var tmp = muzzleFlashLocation.localScale;
        tmp.y = -tmp.y;
        muzzleFlashLocation.localScale = tmp;
    }
    public void HideMuzzleFlash() => muzzleFlashLocation.gameObject.SetActive(false);

    void Start() {
        health = maxHealth;
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

    public bool CanSee(Vector2 gridLocation) => Map.instance.CanBeSeenFrom(new SoldierLosMask(), gridLocation, this.gridLocation);

    public void Destroy() {
        Destroy(gameObject);
    }

    public override void Interact(Tile tile) {
        if (!tile.open) {
            UIState.instance.DeselectActor();
            MapHighlighter.instance.ClearHighlights();
            return;
        }
        if (tile.GetActor<Actor>() == null) {
            var path = Map.instance.ShortestPath(new SoldierImpassableTerrain(), gridLocation, tile.gridLocation);
            if (path.length <= remainingMovement) {
                AnimationManager.instance.StartAnimation(GameplayOperations.PerformActorMove(this, tile.gridLocation));
                tilesMoved += path.length;
                HighlightActions();
            } else {
                UIState.instance.DeselectActor();
                MapHighlighter.instance.ClearHighlights();
            }
        } else {
            var alien = tile.GetActor<Alien>();
            if (alien != null && CanSee(alien.gridLocation) && canAct) {
                MapHighlighter.instance.ClearHighlights();
                Shoot(alien);
                actionsSpent += 1;
            }
        }
    }

    public void HighlightActions() {
        MapHighlighter.instance.ClearHighlights();
        foreach (var tile in Map.instance.iterator.Exclude(new SoldierImpassableTerrain()).RadiallyFrom(gridLocation, remainingMovement)) {
            MapHighlighter.instance.HighlightTile(tile, Color.green);
        }
        if (canAct) {
            foreach (var alien in Map.instance.GetActors<Alien>()) {
                if (CanSee(alien.gridLocation)) {
                    MapHighlighter.instance.HighlightTile(alien.tile, Color.red);
                }
            }
        }
    }

    public override void Select() {
        UIState.instance.SetSelectedActor(this);
        HighlightActions();
    }
}

public class SoldierImpassableTerrain : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetActor<Alien>() != null;
    }
}

public class SoldierLosMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open;
    }
}