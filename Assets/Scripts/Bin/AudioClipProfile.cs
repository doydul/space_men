using UnityEngine;

[System.Serializable]
public class AudioClipProfile {
    [Range(0f, 1f)]
    public float volume;
    public AudioClip clip;
}