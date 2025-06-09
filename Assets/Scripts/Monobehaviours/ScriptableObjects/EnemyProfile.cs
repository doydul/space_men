using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public enum EnemyProfileType {
    Generic,
    Group,
    Armoured,
    Big,
    Quick
}

[System.Serializable]
public enum EnemyProfileCondition {
    Or,
    And
}

[System.Serializable]
public struct EnemyProfileTypeInfo {
    public EnemyProfileType type;
    [Range(0, 100)]
    public int typeWeight;

    public bool Matches(PlayerSave save) {
        if (type == EnemyProfileType.Group) {
            if (typeWeight <= save.groupishness) return true;
        } else if (type == EnemyProfileType.Armoured) {
            if (typeWeight <= save.armouredness) return true;
        } else if (type == EnemyProfileType.Big) {
            if (typeWeight <= save.bigness) return true;
        } else if (type == EnemyProfileType.Quick) {
            if (typeWeight <= save.quickness) return true;
        } else {
            int genericValue = 100 - save.groupishness - save.armouredness - save.quickness - save.bigness;
            if (typeWeight <= genericValue) return true;
        }
        return false;
    }
}

[System.Serializable]
public struct EnemyProfileGroupSize {
    public EnemyProfileTypeInfo typeInfo;
    public int groupSize;
}

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Enemy Profile", order = 3)]
public class EnemyProfile : ScriptableObject, IWeighted {

    public string typeName; 
    public int difficultyLevel;
    public int subLevel;
    public int threat;
    public int weight;
    public int Weight => weight;
    public EnemyProfileCondition typeCondition;
    public EnemyProfileTypeInfo[] typeInfo;
    public EnemyProfileGroupSize[] groupSizeUnlocks;
    [Range(0, 100)]
    public int spawnPercentage;
    
    public AlienData data => AlienData.Get(typeName);

    public static EnemyProfile none => Resources.Load<EnemyProfile>("Aliens/SpecialProfiles/None");
    public static EnemyProfile[] GetAll() => Resources.LoadAll("Aliens/Profiles", typeof(EnemyProfile)).Select(prof => prof as EnemyProfile).ToArray();

    public int BestScore(PlayerSave save) {
        var matchingWeights = typeInfo.Where(info => info.Matches(save)).Select(info => info.typeWeight);
        if (typeCondition == EnemyProfileCondition.And) {
            return matchingWeights.Sum();
        } else {
            return matchingWeights.Max();
        }
    }

    public bool Fits(PlayerSave save) {
        if (typeCondition == EnemyProfileCondition.And) {
            return typeInfo.All(info => info.Matches(save));
        } else {
            return typeInfo.Any(info => info.Matches(save));
        }
    }

    public int AvailableGroupSize(PlayerSave save) {
        var matchingGroupSizes = groupSizeUnlocks.Where(gs => gs.typeInfo.Matches(save));
        return matchingGroupSizes.Count() > 0 ? matchingGroupSizes.MaxBy(gs => gs.groupSize).groupSize : 1;
    }
}