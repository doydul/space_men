using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignUI : MonoBehaviour {

    public Transform squadSoldierPrototype;

    SoldierComponent soldierComponent;

    void Start() {
        squadSoldierPrototype.gameObject.SetActive(false);
        soldierComponent = GetComponentInChildren<SoldierComponent>(true);

        DisplaySquad();
    }

    void DisplaySquad() {
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