using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignUI : MonoBehaviour {
    
    public static CampaignUI instance;

    public Transform squadSoldierPrototype;

    SoldierComponent soldierComponent;
    
    void Awake() => instance = this;

    void Start() {
        squadSoldierPrototype.gameObject.SetActive(false);
        soldierComponent = GetComponentInChildren<SoldierComponent>(true);

        DisplaySquad();
    }

    public void DisplaySquad() {
        squadSoldierPrototype.parent.DestroyChildren(startIndex: 1);
        foreach (var soldier in PlayerSave.current.squad.GetMetaSoldiers()) {
            var squadSoldierTrans = Instantiate(squadSoldierPrototype, squadSoldierPrototype.parent);
            squadSoldierTrans.gameObject.SetActive(true);

            var buttonHandler = squadSoldierTrans.GetComponentInChildren<ButtonHandler>();
            buttonHandler.action.AddListener(() => {
                soldierComponent.Open(soldier);
            });
        }
    }

    public void NextLevel() {
        SceneManager.LoadScene("Mission");
    }
}