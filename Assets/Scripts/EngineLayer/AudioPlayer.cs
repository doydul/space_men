using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;
    
    void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.minDistance = 500f;

    }

    public void PlayAudio(AudioClipProfile clip) {
        if (clip == null) return;
        audioSource.pitch = Random.value * 0.2f + 0.9f;
        audioSource.volume = clip.volume;
        audioSource.PlayOneShot(clip.clip);
    }
}