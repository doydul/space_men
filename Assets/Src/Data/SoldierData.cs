using UnityEngine;
using System;

[Serializable]
public class SoldierData {
    
    public const string DEFAULT_ARMOUR = "Basic";
    public const string DEFAULT_WEAPON = "Assault Rifle";
    
    public string armour;
    public string weapon;
    
    public void ToSoldier(Soldier soldier) {
        soldier.armour = Resources.Load<Armour>("Armour/" + armour);
        soldier.weapon = Resources.Load<Weapon>("Weapons/" + weapon);
    }
    
    public static SoldierData GenerateDefault() {
        var result = new SoldierData();
        result.armour = DEFAULT_ARMOUR;
        result.weapon = DEFAULT_WEAPON;
        return result;
    }
}