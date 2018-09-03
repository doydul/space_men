using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Armour", menuName = "Scriptable Objects/Armour", order = 1)]
public class Armour : ScriptableObject {
    
    public enum Type {
        Light,
        Medium,
        Heavy
    }
    
    public int armourValue;
    public int movement;
    public int sprint;
    public Type type;
}