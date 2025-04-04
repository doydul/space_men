using System.Collections;
using UnityEditor;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;
    bool repeating;
    
    void Awake() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.minDistance = 500f;

    }

    public void PlayAudio(AudioClipProfile clip, bool randomPitch = true) {
        if (clip == null || repeating) return;
        audioSource.pitch = randomPitch ? Random.value * 0.2f + 0.9f : 1;
        audioSource.volume = clip.volume;
        audioSource.PlayOneShot(clip.clip);
    }

    public void PlayAudioRepeat(AudioClipProfile clip) {
        if (clip == null) return;
        audioSource.pitch = 1;
        audioSource.volume = clip.volume;
        audioSource.clip = clip.clip;
        audioSource.loop = true;
        audioSource.Play();
        repeating = true;
    }
    
    public void StopRepeatingAudio() {
        repeating = false;
        audioSource.Stop();
        audioSource.loop = false;
    }
    
    public IEnumerator PerformPlayAudio(AudioClipProfile clip, bool randomPitch = true) {
        PlayAudio(clip, randomPitch);
        yield return null;
        while (audioSource.isPlaying) {
            yield return null;
        }
    }
}