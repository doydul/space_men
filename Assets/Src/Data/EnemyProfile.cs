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

    public bool Matches(EnemyProfileSet set) {
        if (type == EnemyProfileType.Group) {
            if (typeWeight <= set.groupishness) return true;
        } else if (type == EnemyProfileType.Armoured) {
            if (typeWeight <= set.armouredness) return true;
        } else if (type == EnemyProfileType.Big) {
            if (typeWeight <= set.bigness) return true;
        } else if (type == EnemyProfileType.Quick) {
            if (typeWeight <= set.quickness) return true;
        } else {
            int genericValue = 100 - set.groupishness - set.armouredness - set.quickness - set.bigness;
            if (typeWeight <= genericValue) return true;
        }
        return false;
    }
}

[System.Serializable]
public struct EnemyProfileGroupSize {
    public EnemyProfileType type;
    [Range(0, 100)]
    public int typeWeight;
    public int groupSize;
}

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Enemy Profile", order = 3)]
public class EnemyProfile : ScriptableObject, IWeighted {

    public bool unlocked;
    public string typeName; 
    public int difficultyLevel;
    public int threat;
    public int weight;
    public int Weight => weight;
    public EnemyProfileCondition typeCondition;
    public EnemyProfileTypeInfo[] typeInfo;
    public EnemyProfileGroupSize[] groupSizeUnlocks;
    [Range(0, 100)]
    public int spawnPercentage;

    public static EnemyProfile none => Resources.Load<EnemyProfile>("Aliens/SpecialProfiles/None");
    public static EnemyProfile[] GetAll() => Resources.LoadAll("Aliens/Profiles", typeof(EnemyProfile)).Select(prof => prof as EnemyProfile).ToArray();

    public int BestScore(EnemyProfileSet set) {
        var matchingWeights = typeInfo.Where(info => info.Matches(set)).Select(info => info.typeWeight);
        if (typeCondition == EnemyProfileCondition.And) {
            return matchingWeights.Sum();
        } else {
            return matchingWeights.Max();
        }
    }

    public bool Fits(EnemyProfileSet set) {
        if (typeCondition == EnemyProfileCondition.And) {
            return typeInfo.All(info => info.Matches(set));
        } else {
            return typeInfo.Any(info => info.Matches(set));
        }
    }
}