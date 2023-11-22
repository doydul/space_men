using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "Enemy Profile", order = 3)]
public class EnemyProfile : ScriptableObject {


    public string typeName; 
    public int count;
    public int difficultyLevel;
    public int threat;
    public int weight;
    public bool spawnable;

    public static EnemyProfile[] GetAll() => Resources.LoadAll("Aliens/Profiles", typeof(EnemyProfile)).Select(prof => prof as EnemyProfile).ToArray();
}

public static class EnemyProfileExtensions {
    public static EnemyProfile WeightedSelect(this IEnumerable<EnemyProfile> profiles) {
        int sum = profiles.Select(prof => prof.weight).Sum();
        var rand = Random.Range(0, sum) + 1;
        foreach (var prof in profiles) {
            rand -= prof.weight;
            if (rand <= 0) return prof;
        }
        return profiles.Last();
    }
}