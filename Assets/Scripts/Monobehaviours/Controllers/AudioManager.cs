using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    
    static AudioManager instance;
    
    AudioLibrary library;
    
    public static AudioPlayer Play(AudioClipProfile profile) {
        return Play(Camera.main.transform.position, profile);
    }
    
    public static AudioPlayer Play(Vector2 location, AudioClipProfile profile) {
        var result = instance.MakePlayer();
        result.transform.position = location;
        result.PlayAudio(profile);
        result.DestroyWhenFinished();
        return result;
    }
    
    public static AudioPlayer Play(AudioCollection collection) {
        return Play(collection.Sample());
    }
    
    public static AudioPlayer Play(Vector2 location, AudioCollection collection) {
        return Play(location, collection.Sample());
    }
    
    public static AudioPlayer PlayRepeating(Vector2 location, AudioClipProfile profile) {
        var result = instance.MakePlayer();
        result.transform.position = location;
        result.PlayAudioRepeat(profile);
        return result;
    }
    
    public static IEnumerator PerformPlay(Vector2 location, AudioClipProfile profile) {
        var result = instance.MakePlayer();
        result.transform.position = location;
        result.DestroyWhenFinished();
        yield return result.PerformPlayAudio(profile);
    }
    
    public static void ButtonClick() => Play(instance.library.buttonClicks);
    public static void ObjectiveComplete() => Play(instance.library.objectiveComplete);
    
    void Awake() {
        instance = this;
        library = Resources.Load<AudioLibrary>("Audio/Library");
    }
    
    AudioPlayer MakePlayer() {
        return Instantiate(Resources.Load<AudioPlayer>("Prefabs/AudioPlayer"));
    }
}