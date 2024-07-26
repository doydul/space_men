using UnityEngine;

public class DebugController : MonoBehaviour {
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ModalPopup.instance.DisplayEOL();
        }
    }
}