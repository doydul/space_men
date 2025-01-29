using UnityEngine;

public class SoldierIconHeader : MonoBehaviour {
    
    public static SoldierIconHeader instance;
    
    public Transform soldierIconPrototype;
    
    void Awake() => instance = this;
    
    void Start() {
        soldierIconPrototype.gameObject.SetActive(false);
    }
    
    public void DisplaySoldiers() {
        soldierIconPrototype.parent.DestroyChildren(startIndex: 2);
        foreach (var soldier in Map.instance.GetActors<Soldier>()) {
            var squadSoldierTrans = Instantiate(soldierIconPrototype, soldierIconPrototype.parent);
            squadSoldierTrans.gameObject.SetActive(true);

            var soldierIcon = squadSoldierTrans.GetComponentInChildren<SoldierIcon>();
            soldierIcon.ShowSoldier(soldier);
        }
    }
    
    public void Show() {
        gameObject.SetActive(true);
    }
    
    public void Hide() {
        gameObject.SetActive(false);
    }
}