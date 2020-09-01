using UnityEngine;

public class Testi : MonoBehaviour {
    public ScriptingController controller;
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) controller.SpawnAliens("Alien", 12, 31, Data.Direction.Left);
    }
}