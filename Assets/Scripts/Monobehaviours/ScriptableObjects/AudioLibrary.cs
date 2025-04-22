using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Library", menuName = "Audio/Audio Library", order = 1)]
public class AudioLibrary : ScriptableObject {
    public AudioCollection buttonClicks;
    public AudioClipProfile objectiveComplete;
}