using UnityEngine;

public class Gaussian {

    private float mean;
    private float stdDev;

    public Gaussian(float mean, float stdDev) {
        this.mean = mean;
        this.stdDev = stdDev;
    }

    public float value { get {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt((float)(-2.0 * Mathf.Log(u1))) *
                     Mathf.Sin((float)(2.0 * Mathf.PI * u2));
        return mean + stdDev * randStdNormal;
    } }
}
