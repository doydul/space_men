using System.Linq;
using System;
using UnityEngine;

using Data;

public class SoldierStore : ISoldierStore {
    
    public ArmourStats GetArmourStats(string armourName) {
        var data = GetArmourData(armourName);
        return new ArmourStats {
            armourValue = data.armourValue,
            weight = ConvertArmourWeight(data.type),
            cost = data.value
        };
    }

    public WeaponStats GetWeaponStats(string weaponName) {
        var data = GetWeaponData(weaponName);
        return new WeaponStats {
            accuracy = data.accuracy,
            armourPen = data.armourPen,
            minDamage = data.minDamage,
            maxDamage = data.maxDamage,
            shotsWhenMoving = data.shotsWhenMoving,
            shotsWhenStill = data.shotsWhenStill,
            blast = data.blast,
            value = data.value
        };
    }
    
    Armour GetArmourData(string armourName) {
        return Resources.Load<Armour>("Armour/" + armourName);
    }

    Weapon GetWeaponData(string weaponName) {
        return Resources.Load<Weapon>("Weapons/" + weaponName);
    }

    ArmourWeight ConvertArmourWeight(Armour.Type type) {
        if (type == Armour.Type.Light) {
            return ArmourWeight.Light;
        } else if (type == Armour.Type.Medium) {
            return ArmourWeight.Medium;
        } else {
            return ArmourWeight.Heavy;
        }
    }
}
