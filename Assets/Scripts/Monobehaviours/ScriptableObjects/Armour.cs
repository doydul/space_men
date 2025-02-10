using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Armour", menuName = "Equipment/Armour", order = 1)]
public class Armour : ScriptableObject, IWeighted {

    public enum Type {
        Light,
        Medium,
        Heavy
    }

    public Transform icon;

    public int techLevel = 1;
    public int weight = 100;
    public int Weight => weight;

    public int armourValue;
    public Type type;
    public int cost;
    [TextArea] public string description;
    public int maxHealth;
    public int sightRange;
    public Ability[] abilities;
    public Trait[] traits;
    public Sprite headSprite;
    public Sprite bodySprite;

    public int movement { get {
        switch(type) {
            case Type.Light:
                return 6;
            case Type.Medium:
                return 5;
            case Type.Heavy:
                return 4;
        }
        return 3;
    } }

    public int sprint { get {
        switch(type) {
            case Type.Light:
                return 6;
            case Type.Medium:
                return 4;
            case Type.Heavy:
                return 4;
        }
        return 3;
    } }

    public bool isLight { get { return type == Type.Light; } }
    public bool isMedium { get { return type == Type.Medium; } }
    public bool isHeavy { get { return type == Type.Heavy; } }

    public static Armour Get(string name) {
        return Resources.Load<Armour>("Armour/" + name);
    }
    
    public string GetFullDescription() {
        string result = $"{name}\ntype: {type}\nhealth points: {maxHealth}\nmovement: {movement}\nsprint: {sprint}\nsight range: {sightRange}\nvalue: {cost}";
        return result;
    }
}
