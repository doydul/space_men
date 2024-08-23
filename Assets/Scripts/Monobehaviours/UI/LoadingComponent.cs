using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingComponent : MonoBehaviour {
    
    public void Load(int slot) {
        PlayerSave.current = PlayerSave.Load(slot);
        SceneManager.LoadScene("CampaignUI");
    }
    
    public void Back() {
        SceneManager.LoadScene("MainMenu");
    }
}