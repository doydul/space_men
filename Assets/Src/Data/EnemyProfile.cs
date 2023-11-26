using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Enemy Profile", order = 3)]
public class EnemyProfile : ScriptableObject, IWeighted {


    public string typeName; 
    public int count;
    public int difficultyLevel;
    public int threat;
    public int weight;
    public int Weight => weight;
    public bool spawnable;

    public static EnemyProfile[] GetAll() => Resources.LoadAll("Aliens/Profiles", typeof(EnemyProfile)).Select(prof => prof as EnemyProfile).ToArray();
}

public static class EnemyProfileExtensions {
    public static T WeightedSelect<T>(this IEnumerable<T> profiles) where T : IWeighted {
        int sum = profiles.Select(prof => prof.Weight).Sum();
        var rand = Random.Range(0, sum) + 1;
        foreach (var prof in profiles) {
            rand -= prof.Weight;
            if (rand <= 0) return prof;
        }
        return profiles.Last();
    }
}