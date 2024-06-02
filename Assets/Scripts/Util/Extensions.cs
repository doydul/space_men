using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static T Sample<T>(this IEnumerable<T> list) {
        if (list.Count() <= 0) return default(T);
        return list.ElementAt(Random.Range(0, list.Count()));
    }

    public static List<T> Sample<T>(this IEnumerable<T> list, int num) {
        if (list.Count() <= 0) return new List<T>();
        var tmpList = new List<T>(list);
        var resultList = new List<T>();
        for (int i = 0; i < num; i++) {
            var el = tmpList.Sample();
            resultList.Add(el);
            tmpList.Remove(el);
            if (tmpList.Count <= 0) return resultList;
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

    public static T MaxBy<T>(this IEnumerable<T> enumerable, System.Func<T, float> valueFunction) {
        T result = default(T);
        float max = -9999999;
        foreach (T obj in enumerable) {
            float value = valueFunction(obj);
            if (value > max) {
                max = value;
                result = obj;
            }
        }
        return result;
    }

    public static void DestroyChildren(this Transform parent, int startIndex = 0) {
        for (int i = startIndex; i < parent.childCount; i++) {
            MonoBehaviour.Destroy(parent.GetChild(i).gameObject);
        }
    }
}