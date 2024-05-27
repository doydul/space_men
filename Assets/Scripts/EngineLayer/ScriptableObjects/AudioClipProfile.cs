using UnityEngine;

[System.Serializable]
public class AudioClipProfile {
    [Range(0f, 1f)]
    public float volume = 0.3f;
    public AudioClip clip;
}