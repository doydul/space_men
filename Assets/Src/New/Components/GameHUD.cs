using UnityEngine;

public class GameHUD : MonoBehaviour {

    void Start() {
        Close();
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void Open() {
        gameObject.SetActive(true);
    }
}