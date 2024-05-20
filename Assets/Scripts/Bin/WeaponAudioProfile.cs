using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WeaponAudio", menuName = "Audio/Weapon Audio", order = 2)]
public class WeaponAudioProfile : ScriptableObject {

    public AudioClip shoot;
    public AudioClip reload;
    public AudioCollection hit;
    public AudioCollection miss;
    public AudioClip explosion;
}
