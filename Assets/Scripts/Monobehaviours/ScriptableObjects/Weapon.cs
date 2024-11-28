using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/Weapon", order = 2)]
public class Weapon : ScriptableObject, IWeighted {

    public enum Type {
        Standard,
        Heavy
    }

    public Transform icon;

    public int techLevel = 1;
    public int weight = 100;
    public int Weight => weight;
    public int accuracy;
    public int minDamage;
    public int maxDamage;
    public DamageType damageType;
    public int shots;
    public int ammo;
    public int range;
    public float blast;
    public bool flames;
    public int flameDuration;
    public Color flameColor;
    public Color muzzleFlareColor = Color.white;
    public Type type;
    public int cost;
    [TextArea] public string description;
    public Ability[] abilities;
    public Trait[] traits;
    public GameObject weaponPrefab;
    public GameObject tracerPrefab;
    public WeaponAudioProfile audio;
    public ParticleBurst impactEffect;
    public ParticleBurst missEffect;

    public bool ordnance { get { return blast > 0; } }
    public bool isHeavy { get { return type == Type.Heavy; } }

    public static Weapon Get(string name) {
        return Resources.Load<Weapon>("Weapons/" + name);
    }
    
    public bool InRange(Vector2 shooterLocation, Vector2 targetLocation) => Map.instance.ManhattanDistance(shooterLocation, targetLocation) <= range;
    
    public string GetFullDescription() {
        string result = $"{name}\naccuracy: {accuracy}";
        if (blast == 0) result += $"\nshots: {shots}";
        else result += $"\nblast: {blast}";
        result += $"\ndamage: {minDamage}-{maxDamage}\nclip size: {ammo}";
        if (damageType == DamageType.Energy) result += $"\ntrait: energy";
        if (type == Type.Heavy) result += $"\ntrait: heavy";
        result += $"\nvalue: {cost}";
        return result;
    }
}
