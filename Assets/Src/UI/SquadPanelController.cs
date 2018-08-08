using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonHandler))]
public class SquadPanelController : MonoBehaviour {
    
    public SquadSelectMenuController menuController;
    public Text buttonText;
    
    private ButtonHandler buttonHandler;
    
    private Squad _squad;
    public Squad squad {
        set {
            _squad = value;
            buttonText.text = _squad.name;
            var sol = _squad.soldiers[0];
        }
    }
    
    void Awake() {
        buttonHandler = GetComponent<ButtonHandler>();
        buttonHandler.action.AddListener(() => {
            SelectSquad();
        });
    }
    
    void SelectSquad() {
        menuController.Next(_squad);
    }
}