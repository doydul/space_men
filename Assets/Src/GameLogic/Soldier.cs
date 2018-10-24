using UnityEngine;
using System;

public class Soldier : Actor {

    public Armour armour;
    public Weapon weapon;
    public SpriteRenderer muzzleFlashRenderer;

    public int health { get; set; }
    private Delayer muzzleDelayer;
    public int tilesMoved { get; set; }
    public int shotsFiredThisRound { get; set; }
    public int shotsFiredFromClip { get; set; }

    public int totalShots { get {
        if (sprinted) {
            return 0;
        } else if (moved) {
            return weapon.shotsWhenMoving;
        } else {
            return weapon.shotsWhenStill;
        }
    } }
    public int shotsRemaining { get { return totalShots - shotsFiredThisRound; } }
    public bool hasAmmo { get { return shotsRemaining > 0; } }
    public int baseMovement { get { return armour.movement; } }
    public int totalMovement { get { return baseMovement + armour.sprint; } }
    public int remainingMovement { get { return totalMovement - tilesMoved; } }
    public bool sprinted { get { return tilesMoved > baseMovement; } }
    public bool moved { get { return tilesMoved > 0; } }
    public int accuracy { get { return weapon.accuracy; } }
    public int armourPen { get { return weapon.armourPen; } }
    public int damage { get { return weapon.damage; } }
    public float blast { get { return weapon.blast; } }
    public bool firesOrdnance { get { return weapon.ordnance; } }
    public Weapon.Type weaponType { get { return weapon.type; } }
    public int maxHealth { get { return 86; } }

    public void Select() { GameLogicComponent.userInterface.Select(this); }
    public void Deselect() { GameLogicComponent.userInterface.Deselect(this); }

    void Start() {
        health = 86;
        muzzleDelayer = new Delayer(this);
        muzzleFlashRenderer.enabled = false;
        StartMovementPhase();
    }

    public SoldierData ToData() {
        var data = new SoldierData();
        if (armour != null) data.armour = armour.name;
        if (weapon != null) data.weapon = weapon.name;
        return data;
    }

    public void FromData(SoldierData soldierData) {
        armour = Resources.Load<Armour>("Armour/" + soldierData.armour);
        weapon = Resources.Load<Weapon>("Weapons/" + soldierData.weapon);
    }

    public override void MoveTo(Tile newTile) {
        int distance = (int)(Mathf.Abs(tile.gridLocation.x - newTile.gridLocation.x) + Mathf.Abs(tile.gridLocation.y - newTile.gridLocation.y));
        tilesMoved += distance;
        base.MoveTo(newTile);
    }

    public void Reload() {
        shotsFiredFromClip = 0;
        tilesMoved = 100;
    }

    public void StartMovementPhase() {
        tilesMoved = 0;
        shotsFiredThisRound = 0;
        GameLogicComponent.userInterface.ShowMovementIndicators(this);
    }

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

    public void ExpendAmmo() {
        shotsFiredThisRound += 1;
        shotsFiredFromClip += 1;

        muzzleFlashRenderer.enabled = true;
        muzzleDelayer.Wait(1, () => {
            muzzleFlashRenderer.enabled = false;
        });
    }

    public void Hurt(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject, 1);
        }
    }

    public void ShowHitIndicator() {

    }

    public void ShowDeflectIndicator() {

    }
}
