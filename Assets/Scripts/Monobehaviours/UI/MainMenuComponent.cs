using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuComponent : MonoBehaviour {
    
    public void StartGame() {
        SceneManager.LoadScene("Mission");
    }
    
    public void LoadGame() {
        SceneManager.LoadScene("LoadingMenu");
    }
    
    public void Help() {
        SceneManager.LoadScene("Help");
    }
}