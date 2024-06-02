using UnityEngine;
using System.Collections.Generic;

public static class MathUtil {
    
    public static IEnumerable<float> RandFixedSum(int numValues, float sum) {
        List<float> values = new();
        for (int i = 0; i < numValues - 1; i++) {
            values.Add(Random.Range(0f, sum));
        }
        values.Sort();
        List<float> result = new();
        result.Add(values[0]);
        for (int i = 0; i < numValues - 2; i++) {
            result.Add(values[i + 1] - values[i]);
        }
        result.Add(sum - values[values.Count - 1]);
        return result;
    }
}