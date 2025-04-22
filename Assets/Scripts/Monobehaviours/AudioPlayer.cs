using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

    AudioSource audioSource;
    bool repeating;
    bool destroyOnEnd;
    
    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update() {
        if (destroyOnEnd && !audioSource.isPlaying) Destroy(gameObject);
    }
    
    public void DestroyWhenFinished() => destroyOnEnd = true;

    public void PlayAudio(AudioClipProfile clip, bool randomPitch = false) {
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
        Destroy(gameObject);
    }
    
    public IEnumerator PerformPlayAudio(AudioClipProfile clip, bool randomPitch = false) {
        PlayAudio(clip, randomPitch);
        yield return null;
        while (audioSource.isPlaying) {
            yield return null;
        }
    }
}