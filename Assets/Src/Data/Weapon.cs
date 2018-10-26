using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon", order = 2)]
public class Weapon : ScriptableObject {

    public enum Type {
        Direct,
        Blast,
        Heavy
    }

    public int accuracy;
    public int armourPen;
    public int damage;
    public int shotsWhenMoving;
    public int shotsWhenStill;
    public int ammo;
    public float blast;
    public Type type;
    public bool ordnance { get { return blast > 0; } }
}
