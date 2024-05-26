using UnityEngine;
using System;

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

    public bool ordnance { get { return blast > 0; } }
    public bool isHeavy { get { return type == Type.Heavy; } }

    public static Weapon Get(string name) {
        return Resources.Load<Weapon>("Weapons/" + name);
    }
}
