using System;

[Serializable]
public class MissionReward {

    public Type type;
    public string itemName;
    public int credits;

    public enum Type {
        Credits,
        Weapon,
        Armour,
        Soldier
    }
}