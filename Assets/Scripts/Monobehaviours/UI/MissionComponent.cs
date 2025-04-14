using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class MissionComponent : MonoBehaviour {
    
    public TMP_Text objectiveTextElement;
    
    void Start() {
        objectiveTextElement.text = "";
        foreach (var objectiveData in Mission.current.objectives.Where(objc => objc.required)) {
            objectiveTextElement.text += $"- {objectiveData.Dump().description}\n";
        }
        Close();
    }
    
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