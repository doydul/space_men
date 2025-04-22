using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Actor : MonoBehaviour {

    public enum Direction {
        Up,
        Left,
        Down,
        Right
    }
    
    public static Quaternion DirectionToRotation(Direction direction) => Quaternion.Euler(0, 0, (int)direction * 90);
    public static Direction FacingToDirection(Vector2 facing) {
        if (Mathf.Abs(facing.y) > Mathf.Abs(facing.x)) {
            if (facing.y > 0) {
                return Direction.Up;
            } else if (facing.y < 0) {
                return Direction.Down;
            }
        } else {
            if (facing.x > 0) {
                return Direction.Right;
            } else if (facing.x < 0) {
                return Direction.Left;
            }
        }
        return Direction.Up;
    }
    public static Direction RandomDirection() {
        var rand = Random.value;
        if (rand < 0.25f) return Direction.Up;
        else if (rand < 0.5f) return Direction.Down;
        else if (rand < 0.75f) return Direction.Left;
        else return Direction.Right;
    }

    [HideInInspector] public Tile tile;
    public Transform image;
    public Decal[] bloodSplats;
    public HealthIndicator healthIndicator;
    public StatusesBar statusesBar;
    public AudioCollection walkSounds;
    public AudioCollection hurtSounds;
    public AudioCollection dieSounds;
    public ParticleBurst hitEffect;
    
    protected List<StatusEffect> statuses = new();
    public float damageMultiplier { get; set; } = 1f;
    public float armourMultiplier { get; set; } = 1f;

    public string id { get; set; }
    public long index { get; set; } // remove me
    public Direction direction { get; private set; }
    public bool dead { get; private set; }
    public virtual bool interactable => false;

    public int maxHealth { get; set; }
    public int health { get; set; }
    public int healthPercentage => health * 100 / maxHealth;
    public int actualTilesMoved { get; set; }
    public Vector2 gridLocation => tile.gridLocation;
    public Vector2 realLocation => transform.position;
    public int rotation => (int)direction * 90;
    public Vector2 facing { get {
        return new Vector2(
            (float)Mathf.Sin(rotation * Mathf.PI / 180),
            (float)Mathf.Cos(rotation * Mathf.PI / 180)
        );
    } }
    public virtual int remainingMovement => 0;
    public bool On(Tile tile) => this.tile == tile;
    protected Animator animator;
    protected Material spriteSharedMat;
    public void PlayAudio(AudioClipProfile clip) => AudioManager.Play(realLocation, clip);
    AudioPlayer repeatingPlayer;
    public void PlayAudioRepeat(AudioClipProfile clip) => repeatingPlayer = AudioManager.PlayRepeating(realLocation, clip);
    public void StopRepeatingAudio() => repeatingPlayer.StopRepeatingAudio();
    public IEnumerator PerformPlayAudio(AudioClipProfile clip) => AudioManager.PerformPlay(realLocation, clip);
    public virtual bool HasTrait(Trait trait) => false;
    
    // Animations
    public void MoveAnimation() => animator?.SetBool("Moving", true);
    public void StationaryAnimation() => animator?.SetBool("Moving", false);

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    private void SetHealthIndicatorSize() {
        healthIndicator.Set(maxHealth, health);
        if (health < maxHealth) healthIndicator.Show();
    }

    public virtual void MoveTo(Tile newTile) {
        if (tile.GetActor() == this) tile.RemoveActor();
        newTile.SetActor(transform);
    }

    public void TurnTo(Direction direction) {
        this.direction = direction;
        image.rotation = DirectionToRotation(direction);
    }

    public void TurnTo(Vector2 direction) {
        TurnTo(FacingToDirection(direction));
    }

    public virtual void Remove() {
        if (tile.GetActor<Actor>() == this) {
            tile.RemoveActor();
        } else {
            tile.RemoveBackgroundActor();
        }
        Destroy(gameObject, 0.5f);
    }

    public virtual bool Hurt(int damage, DamageType damageType = DamageType.Normal) {
        health -= damage;
        if (health <= 0) {
            dead = true;
            PlayAudio(dieSounds.Sample());
            Remove();
        } else {
            PlayAudio(hurtSounds.Sample());
        }
        SetHealthIndicatorSize();
        HurtAnimation();
        return true;
    }
    
    public virtual bool DamageExceedsArmour(int damage, DamageType damageType) => false;

    public void Face(Vector2 targetGridLocation) {
        TurnTo(targetGridLocation - gridLocation);
    }
    
    public virtual void SetSpriteTransform(Transform spriteTransform) {
        spriteTransform.parent = image;
        spriteTransform.localPosition = Vector3.zero;
        SetupSpriteMaterials();
    }
    
    public void SetupSpriteMaterials() {
        var masterMat = Resources.Load<Material>("Materials/DamageableSprite");
        spriteSharedMat = new Material(masterMat);
        foreach (var renderer in image.GetComponentsInChildren<SpriteRenderer>()) {
            if (renderer.gameObject.tag != "HDR") renderer.sharedMaterial = spriteSharedMat;
        }
        SetHurtOverlayIntensity(0);
    }
    
    Coroutine hurtAnimCoroutine;
    public void HurtAnimation() {
        if (hurtAnimCoroutine != null) AnimationManager.instance.StopCoroutine(hurtAnimCoroutine);
        hurtAnimCoroutine = AnimationManager.instance.StartCoroutine(PerformHurtAnimation());
    }
    
    public IEnumerator PerformHurtAnimation() {
        float duration = 0.75f;
        float t = 0;
        float startIntensity = 0.75f;
        while (t < 1) {
            SetHurtOverlayIntensity(startIntensity * (1 - t));
            t += Time.deltaTime / duration;
            yield return null;
        }
        SetHurtOverlayIntensity(0);
    }
    
    public void SetHurtOverlayIntensity(float intensity) => spriteSharedMat.SetFloat("_HurtIntensity", intensity);
    
    public void SpawnBlood() {
        DecalController.instance.SpawnDecal(tile, bloodSplats.Sample());
    }

    public virtual void Interact(Tile tile) {}
    public virtual IEnumerator PerformUse(Soldier user) { yield return null; }

    public virtual void Select() {}
    public virtual void Deselect() {}
    
        
    public void AddStatus(StatusEffect status) {
        if (HasStatus(status)) RemoveStatus(statuses.First(stat => stat.GetType() == status.GetType()));
        statuses.Add(status);
        statusesBar.DisplayStatuses(statuses);
        healthIndicator.Show();
    }
    public void RemoveStatus(StatusEffect status) {
        statuses.Remove(status);
        statusesBar.DisplayStatuses(statuses);
        if (statuses.Count <= 0) {
            healthIndicator.Hide();
            SetHealthIndicatorSize();
        }
    }
    public bool HasStatus<T>() => statuses.Where(stat => stat.GetType() == typeof(T)).Any();
    public bool HasStatus(StatusEffect status) => statuses.Where(stat => stat.GetType() == status.GetType()).Any();
    
    public void StartOfTurn() {
        foreach (var status in new List<StatusEffect>(statuses)) status.StartOfTurn();
    }
    public void EndOfTurn() {
        foreach (var status in new List<StatusEffect>(statuses)) status.EndOfTurn();
    }
}
