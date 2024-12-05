using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Soldier : Actor {

    public Armour armour;
    Weapon _weapon;
    public Weapon weapon { get => _weapon; set {
        _weapon = value;
        gunContainer.DestroyChildren();
        var weaponPrefab = Instantiate(_weapon.spritePrefab, gunContainer);
        weaponPrefab.transform.localPosition = Vector3.zero;
        muzzleFlashLocation = weaponPrefab.muzzle;
    } }
    Transform muzzleFlashLocation;
    public Transform gunContainer;
    public SpriteRenderer bodySprite;
    public AbilityIcon abilityIcon;
    
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
    public int shotsRemaining => ammo - shotsSpent;
    public bool hasAmmo => shotsRemaining > 0;
    public int baseMovement => armour.movement;
    public int sprintMovement => armour.sprint;
    public int totalMovement => baseMovement + armour.sprint;
    public int remainingMovement => baseMovement - tilesMoved;
    public bool sprinted => tilesMoved > baseMovement;
    public bool moved => tilesMoved > 0;
    public int accuracy => weapon.accuracy;
    public int range => weapon.range;
    public int halfRange => weapon.range / 2;
    public int minDamage => weapon.minDamage;
    public int maxDamage => weapon.maxDamage;
    public int shots => weapon.shots;
    public float blast => weapon.blast;
    public bool firesOrdnance => weapon.ordnance;
    public Weapon.Type weaponType => weapon.type;
    public Vector3 muzzlePosition => new Vector3(muzzleFlashLocation.position.x, muzzleFlashLocation.position.y, tile.transform.position.z);
    public Vector3 centrePosition => new Vector3(transform.position.x, transform.position.y, tile.transform.position.z);

    public bool CanSee(Vector2 gridLocation) => Map.instance.CanBeSeenFrom(new SoldierLosMask(), gridLocation, this.gridLocation);
    public bool InRange(Vector2 gridLocation) => weapon.InRange(this.gridLocation, gridLocation);
    public bool InHalfRange(Vector2 gridLocation) => Map.instance.ManhattanDistance(this.gridLocation, gridLocation) <= halfRange;
    public void Shoot(Alien target) => AnimationManager.instance.StartAnimation(GameplayOperations.PerformSoldierShoot(this, target));
    public IEnumerator PerformShoot(Alien target) => GameplayOperations.PerformSoldierShoot(this, target);
    public bool HasAbility<T>() => abilities.Any(ability => ability is T);
    public bool HasTrait(Trait trait) => weapon.traits != null && weapon.traits.Contains(trait) || armour.traits != null && armour.traits.Contains(trait);
    
    // Animations
    public void AimAnimation() => animator.SetBool("Aiming", true);
    public void IdleAnimation() => animator.SetBool("Aiming", false);
    
    public string armourName => armour.name;
    public string weaponName => weapon.name;

    public List<Ability> abilities = new();

    public void ShowMuzzleFlash() {
        muzzleFlashLocation.gameObject.SetActive(true);
        var tmp = muzzleFlashLocation.localScale;
        tmp.y = -tmp.y;
        muzzleFlashLocation.localScale = tmp;
    }
    public void ShowAbilityIcon(Ability ability) {
        abilityIcon.spriteRenderer.gameObject.SetActive(true);
        abilityIcon.DisplaySpriteFor(ability);
    }
    public void HideAbilityIcon() => abilityIcon.spriteRenderer.gameObject.SetActive(false);

    void Start() {
        HideAbilityIcon();
        GameEvents.On(this, "player_turn_start", Reset);
    }

    void OnDestroy() {
        GameEvents.RemoveListener(this, "player_turn_start");
        foreach (var ability in abilities) ability.Teardown();
    }

    public void GetExp(int amount) {
        exp += amount;
    }

    public void Reset() {
        tilesMoved = 0;
        actionsSpent = 0;
        reaction = null;
        HideAbilityIcon();
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
    
    public override void Remove() {
        base.Remove();
        PlayerSave.current.squad.RemoveMetaSoldierById(id);
        SoldierIconHeader.instance.DisplaySoldiers();
        Mission.CheckGameOver();
    }

    public override void Interact(Tile tile) {
        if (!tile.open) {
            Deselect();
            return;
        }
        if (Map.instance.ManhattanDistance(gridLocation, tile.gridLocation) == 1 && tile.GetBackgroundActor<Actor>() != null && tile.GetBackgroundActor<Actor>().interactable) {
            AnimationManager.instance.StartAnimation(tile.GetBackgroundActor<Actor>().PerformUse(this));
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
                if (!tile.foggy && HasAbility<StandardShoot>() && InRange(alien.gridLocation) && CanSee(alien.gridLocation) && canShoot) {
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
                if (!alien.tile.foggy && CanSee(alien.gridLocation) && InRange(alien.gridLocation)) {
                    MapHighlighter.instance.HighlightTile(alien.tile, Color.red);
                }
            }
        }
        foreach (var tile in Map.instance.AdjacentTiles(tile)) {
            if (tile.GetBackgroundActor<Actor>() != null && tile.GetBackgroundActor<Actor>().interactable) MapHighlighter.instance.HighlightTile(tile, Color.yellow);
        }
        MapHighlighter.instance.HighlightTile(this.tile, new Color(0.75f, 1f, 0.75f));
    }

    public override void Select() {
        UIState.instance.SetSelectedActor(this);
        RefreshUI();
        Tutorial.Show(transform, "select_soldier");
    }

    public override void Deselect() {
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
        return !tile.open || tile.GetActor<Alien>() != null || tile.GetBackgroundActor<Door>() != null || tile.GetBackgroundActor<Chest>() != null;
    }
}

public class SoldierLosMask : IMask {
    public bool Contains(Tile tile) {
        return !tile.open || tile.GetBackgroundActor<Door>() != null;
    }
}