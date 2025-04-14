using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuComponent : MonoBehaviour {
    
    public static int selectedSlot;
    public ButtonHandler continueButton;
    public GameObject newGameConfirmationPanel;
    
    void Start() {
        newGameConfirmationPanel.SetActive(false);
        if (PlayerSave.Load(0) == null) continueButton.Disable();
    }
    
    public void NewGame() {
        if (PlayerSave.Load(0) == null) {
            PlayerSave.current = null;
            SceneManager.LoadScene("Mission");
        } else {
            newGameConfirmationPanel.SetActive(true);
        }
    }
    
    public void ConfirmNewGame() {
        PlayerSave.current = null;
        SceneManager.LoadScene("Mission");
    }
    
    public void CancelNewGame() {
        newGameConfirmationPanel.SetActive(false);
    }
    
    public void Continue() {
        PlayerSave.current = PlayerSave.Load(0);
        SceneManager.LoadScene("CampaignUI");
    }
    
    public void Settings() {
        
    }
}