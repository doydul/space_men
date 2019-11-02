using System.Linq;
using System;
using UnityEngine;

using Data;

public class SoldierStore : ISoldierStore {
    
    public ArmourStats GetArmourStats(ArmourType armourType) {
        var data = GetData(armourType.ToString());
        return new ArmourStats {
            armourValue = data.armourValue,
            weight = ConvertArmourWeight(data.type),
            cost = data.value
        };
    }
    
    Armour GetData(string armourName) {
        return Resources.Load<Armour>("Armour/" + armourName);
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
