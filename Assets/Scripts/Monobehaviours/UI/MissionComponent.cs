using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionComponent : MonoBehaviour {
    
    void Start() => Close();
    
    public void Open() {
        gameObject.SetActive(true);
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void StartMission() {
        SceneManager.LoadScene("Mission");
    }
}