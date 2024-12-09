using UnityEngine;
using System.Collections;

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

    public Tile tile;
    public Transform image;
    public Decal[] bloodSplats;
    public GameObject deflectIndicator;
    public HealthIndicator healthIndicator;
    public AudioCollection walkSounds;
    public AudioCollection hurtSounds;
    public AudioCollection dieSounds;
    public ParticleBurst hitEffect;

    public string id { get; set; }
    public long index { get; set; } // remove me
    public Direction direction { get; private set; }
    public bool dead { get; private set; }
    public bool broken { get; set; }
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
    public bool On(Tile tile) => this.tile == tile;
    protected Animator animator;
    protected Material spriteSharedMat;
    AudioPlayer audioPlayer;
    public void PlayAudio(AudioClipProfile clip) => audioPlayer.PlayAudio(clip);
    
    // Animations
    public void MoveAnimation() => animator?.SetBool("Moving", true);
    public void StationaryAnimation() => animator?.SetBool("Moving", false);

    protected virtual void Awake() {
        audioPlayer = gameObject.AddComponent<AudioPlayer>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    private void SetHealthIndicatorSize() {
        healthIndicator.Set(maxHealth, health);
        if (health < maxHealth) healthIndicator.Show();
    }

    public virtual void MoveTo(Tile newTile) {
        tile.RemoveActor(); 
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
}
