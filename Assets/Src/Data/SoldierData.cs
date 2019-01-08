using UnityEngine;
using System;

[Serializable]
public class SoldierData {

    public const string DEFAULT_ARMOUR = "Basic";
    public const string DEFAULT_WEAPON = "Assault Rifle";

    public string armour;
    public string weapon;

    public int exp;
    public int level;

    public static SoldierData GenerateDefault() {
        var result = new SoldierData();
        result.armour = DEFAULT_ARMOUR;
        result.weapon = DEFAULT_WEAPON;
        return result;
    }

    static int ExpForLevel(int level) {
        return 10 * level * level;
    }

    public int currentLevelExp { get { return ExpForLevel(level); } }
    public int excessExp { get { return exp - currentLevelExp; } }
    public int expToNextLevel { get { return ExpForLevel(level + 1) - exp; } }

    public bool canLevelUp { get { return expToNextLevel <= 0; } }
}
