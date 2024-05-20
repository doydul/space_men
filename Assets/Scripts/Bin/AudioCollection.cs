using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AudioCollection {
    public List<AudioClip> list;

    public AudioClip Sample() => list.Sample();
}