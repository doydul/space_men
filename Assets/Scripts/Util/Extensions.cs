using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static T Sample<T>(this IEnumerable<T> list) {
        return list.ElementAt(Random.Range(0, list.Count()));
    }

    public static List<T> Sample<T>(this IEnumerable<T> list, int num) {
        var tmpList = new List<T>(list);
        if (list.Count() <= num) return tmpList;
        var resultList = new List<T>();
        for (int i = 0; i < num; i++) {
            var el = tmpList.Sample();
            resultList.Add(el);
            tmpList.Remove(el);
        }
        return resultList;
    }

    public static T WeightedSelect<T>(this IEnumerable<T> profiles) where T : IWeighted {
        if (profiles.Count() <= 0)return default(T);
        int sum = profiles.Select(prof => prof.Weight).Sum();
        var rand = Random.Range(0, sum) + 1;
        foreach (var prof in profiles) {
            rand -= prof.Weight;
            if (rand <= 0) return prof;
        }
        return profiles.Last();
    }
}