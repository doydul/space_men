using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioCollection", menuName = "Audio/Audio Collection", order = 1)]
public class AudioCollection : ScriptableObject {
    public List<AudioClipProfile> list;

    public AudioClipProfile Sample() => list.Sample();
}