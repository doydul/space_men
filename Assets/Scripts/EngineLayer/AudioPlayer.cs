using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;
    
    void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.minDistance = 500f;

    }

    public void PlayAudio(AudioClip clip) {
        if (clip == null) return;
        audioSource.pitch = Random.value * 0.2f + 0.9f;
        audioSource.PlayOneShot(clip);
    }
}