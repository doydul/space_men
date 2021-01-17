using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon", order = 2)]
public class Weapon : ScriptableObject {

    public enum Type {
        Standard,
        Heavy
    }

    public Transform icon;

    public int accuracy;
    public int armourPen;
    public int minDamage;
    public int maxDamage;
    public int shotsWhenMoving;
    public int shotsWhenStill;
    public int ammo;
    public float blast;
    public bool flames;
    public int flameDamage;
    public Color flameColor;
    public Type type;
    public int cost;
    [TextArea] public string description;

    public bool ordnance { get { return blast > 0; } }
    public bool isHeavy { get { return type == Type.Heavy; } }

    public static Weapon Get(string name) {
        return Resources.Load<Weapon>("Weapons/" + name);
    }
}
