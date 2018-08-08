using UnityEngine;

[RequireComponent(typeof(ButtonHandler))]
public class TemplarPanelController : MonoBehaviour {
    
    public ButtonHandler buttonHandler;
    public ArmouryMenuController menuController;
    
    void Start() {
        buttonHandler.action.AddListener(() => {
            menuController.ViewTemplar();
        });
    }
}