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
    public Type type;
    public int value;

    public int movement { get {
        switch(type) {
            case Type.Light:
                return 4;
            case Type.Medium:
                return 3;
            case Type.Heavy:
                return 3;
        }
        return 3;
    } }

    public int sprint { get {
        switch(type) {
            case Type.Light:
                return 4;
            case Type.Medium:
                return 3;
            case Type.Heavy:
                return 0;
        }
        return 3;
    } }

    public bool isLight { get { return type == Type.Light; } }
    public bool isMedium { get { return type == Type.Medium; } }
    public bool isHeavy { get { return type == Type.Heavy; } }

    public static Armour Get(string name) {
        return Resources.Load<Armour>("Armour/" + name);
    }
}
