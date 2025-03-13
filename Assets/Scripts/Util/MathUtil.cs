using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

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
    
    public static float EaseCubic(float t) {
        t = Mathf.Clamp01(t);
        return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }
    
    public static float GuassianFloat(float mean, float stdDev) {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
    public static int GuassianInt(float mean, float stdDev) {
        return Mathf.RoundToInt(GuassianFloat(mean, stdDev));
    }
}