using UnityEngine;
using System;

[CreateAssetMenu(fileName = "AlienAudio", menuName = "Audio/Alien Audio", order = 3)]
public class AlienAudioProfile : ScriptableObject {

    public AudioClipProfile attack;
    public AudioClipProfile move;
    public AudioCollection idle;
    public AudioCollection hurt;
    public AudioCollection die;
}