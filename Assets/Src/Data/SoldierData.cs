using UnityEngine;
using System;

[Serializable]
public class SoldierData {

    public const string DEFAULT_ARMOUR = "Basic";
    public const string DEFAULT_WEAPON = "Grenade Launcher";

    public string armour;
    public string weapon;

    public int exp;

    public static SoldierData GenerateDefault() {
        var result = new SoldierData();
        result.armour = DEFAULT_ARMOUR;
        result.weapon = DEFAULT_WEAPON;
        return result;
    }
}
