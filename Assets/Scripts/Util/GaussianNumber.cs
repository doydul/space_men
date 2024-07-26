using UnityEngine;

public class GaussianNumber {
    
    public static float Generate(float mean, float stdv) {
        float u1 = 1f - Random.value;
        float u2 = 1f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        float randNormal = mean + stdv * randStdNormal;
        return randNormal;
    }
}