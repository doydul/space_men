using UnityEngine;

public class AllControllers : MonoBehaviour {

    Controller[] controllers;
    
    void Awake() {
        controllers = FindObjectsOfType<Controller>();
    }

    public void EnableAll() {
        foreach (var controller in controllers) {
            controller.Enable();
        }
    }

    public void DisableAll() {
        foreach (var controller in controllers) {
            controller.Disable();
        }
    }
}