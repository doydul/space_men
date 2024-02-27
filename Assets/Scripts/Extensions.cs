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
}