using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WeaponAudio", menuName = "Audio/Weapon Audio", order = 2)]
public class WeaponAudioProfile : ScriptableObject {

    public AudioClipProfile shoot;
    public AudioClipProfile reload;
    public AudioCollection impact;
    public AudioClipProfile explosion;
}
