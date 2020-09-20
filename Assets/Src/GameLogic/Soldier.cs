using UnityEngine;

public class Soldier : Actor {

    public Armour armour;
    public Weapon weapon;
    public Transform muzzleFlashLocation;
    
    public int tilesMoved { get; set; }
    public int shotsFiredThisRound { get; set; }
    public int shotsFiredFromClip { get; set; }
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
    public float blast { get { return weapon.blast; } }
    public bool firesOrdnance { get { return weapon.ordnance; } }
    public Weapon.Type weaponType { get { return weapon.type; } }
    public Vector2 muzzlePosition { get { return muzzleFlashLocation.position; } }
    
    public string armourName { get { return armour.name; } }
    public string weaponName { get { return weapon.name; } }

    void Start() {
        StartMovementPhase();
    }

    public SoldierData ToData() {
        var data = new SoldierData();
        if (armour != null) data.armour = armourName;
        if (weapon != null) data.weapon = weaponName;
        data.exp = exp;
        data.level = level;
        return data;
    }

    public void FromData(SoldierData soldierData, int index = 0) {
        this.index = index;
        armour = Armour.Get(soldierData.armour);
        weapon = Weapon.Get(soldierData.weapon);
        exp = soldierData.exp;
        level = soldierData.level;
    }

    public void GetExp(int amount) {
        exp += amount;
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
        // tilesMoved = 0;
        // shotsFiredThisRound = 0;
        // if (GameLogicComponent.instance != null) GameLogicComponent.userInterface.ShowMovementIndicators(this);
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

    public void SetAmmo(int ammo) {
        shotsFiredFromClip = ammo;
    }

    public void Hurt(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject, 1);
        }
    }
    
    public void Destroy() {
        Destroy(gameObject);
    }

    public void ShowHitIndicator() {

    }

    public void ShowDeflectIndicator() {

    }
}
