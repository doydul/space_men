using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsMenu : MonoBehaviour {
    
    public Toggle confirmAbilitiesToggle;
    
    void Start() {
        Close();
    }
    
    public void Open() {
        gameObject.SetActive(true);
        confirmAbilitiesToggle.SetState(Settings.confirmAbilities);
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void SetConfirmAbilities(bool state) {
        Settings.confirmAbilities = state;
    }
    
    public void ResetTutorials() {
        Tutorial.ResetAll();
    }
    
    public void Exit() {
        AnimationManager.instance.StartAnimation(PerformExit());
    }
    
    public IEnumerator PerformExit() {
        Close();
        bool confirmed = false;
        yield return NotificationPopup.PerformShow("are you sure?", "the game cannot be saved during a mission, are you sure you want to exit?", new BtnData("cancel", () => {}), new BtnData("ok", () => confirmed = true));
        if (confirmed) SceneManager.LoadScene("MainMenu");
        else Open();
    }
}