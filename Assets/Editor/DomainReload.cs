using UnityEngine;
using UnityEditor;

public static class DomainReload {
    
    [MenuItem("Tools/Reload Domain and Enter Play Mode %o")]
    public static void ReloadDomainAndEnterPlayMode() {
        if (EditorApplication.isPlaying) {
            Debug.LogWarning("Cannot reload the domain while in Play Mode.");
            return;
        }
     
        EditorApplication.playModeStateChanged += ResetPlayModeOptions;
        System.Threading.Thread.Sleep(100);
        EditorApplication.EnterPlaymode();
    }
    
    private static void ResetPlayModeOptions(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingPlayMode) {
            EditorUtility.RequestScriptReload();
            EditorApplication.playModeStateChanged -= ResetPlayModeOptions;
            
        }
    }
}