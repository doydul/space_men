using System.Linq;
using System;
using UnityEngine;

using Data;

public class SoldierStore : ISoldierStore {
    
    public ArmourStats GetArmourStats(string armourName) {
        var data = GetArmourData(armourName);
        if (data == null) return new ArmourStats { cost = -1 };
        return new ArmourStats {
            armourValue = data.armourValue,
            weight = ConvertArmourWeight(data.type),
            cost = data.value,
            maxHealth = data.maxHealth,
            movement = data.movement,
            sprint = data.sprint
        };
    }

    public WeaponStats GetWeaponStats(string weaponName) {
        var data = GetWeaponData(weaponName);
        if (data == null) return new WeaponStats { cost = -1 };
        return new WeaponStats {
            accuracy = data.accuracy,
            armourPen = data.armourPen,
            cost = data.cost,
            minDamage = data.minDamage,
            maxDamage = data.maxDamage,
            shotsWhenMoving = data.shotsWhenMoving,
            shotsWhenStill = data.shotsWhenStill,
            ammo = data.ammo,
            blast = data.blast,
            flames = data.flames,
            flameDamage = data.flameDamage,
            description = data.description
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
