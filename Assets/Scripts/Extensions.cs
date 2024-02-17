using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static T Sample<T>(this IEnumerable<T> list) {
        return list.ElementAt(Random.Range(0, list.Count()));
    }
}