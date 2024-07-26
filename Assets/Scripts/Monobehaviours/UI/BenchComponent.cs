using UnityEngine;
using System.Linq;
using TMPro;

public class BenchComponent : MonoBehaviour {
    
    public SoldierProfileIcon[] soldierProfileIcons;
    public Transform soldierPrototype;
    
    MetaSoldier activeBenchSoldier;
    
    void Start() {
        soldierPrototype.gameObject.SetActive(false);
        Close();
    }
    
    public void Open() {
        DisplaySquadSoldiers();
        DisplayBenchSoldiers();
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
    
    public void ClickSquadSoldier(int index) {
        if (activeBenchSoldier != null) {
            if (index < PlayerSave.current.squad.Count) {
                PlayerSave.current.squad.AddMetaSoldier(activeBenchSoldier);
                PlayerSave.current.bench.Remove(activeBenchSoldier);
                var metaSoldier = PlayerSave.current.squad.RemoveMetaSoldier(index);
                PlayerSave.current.bench.Add(metaSoldier);
            } else {
                PlayerSave.current.squad.AddMetaSoldier(activeBenchSoldier);
                PlayerSave.current.bench.Remove(activeBenchSoldier);
            }
        } else {
            if (index < PlayerSave.current.squad.Count) {
                var metaSoldier = PlayerSave.current.squad.RemoveMetaSoldier(index);
                PlayerSave.current.bench.Add(metaSoldier);
            }
        }
        activeBenchSoldier = null;
        Open();
    }
    
    public void ClickBenchedSoldier(int index) {
        activeBenchSoldier = PlayerSave.current.bench[index];
    }
    
    void DisplaySquadSoldiers() {
        foreach (var icon in soldierProfileIcons) icon.DisplayMetaSoldier(null);
        int index = 0;
        foreach (var metaSoldier in PlayerSave.current.squad.GetMetaSoldiers()) {
            soldierProfileIcons[index].DisplayMetaSoldier(metaSoldier);
            index++;
        }
    }
    
    void DisplayBenchSoldiers() {
        soldierPrototype.parent.DestroyChildren(1);
        
        int index = 0;
        foreach (var metaSoldier in PlayerSave.current.bench) {
            var soldierTrans = Instantiate(soldierPrototype, soldierPrototype.parent);
            soldierTrans.gameObject.SetActive(true);
            var textComponent = soldierTrans.GetComponentInChildren<TMP_Text>();
            textComponent.text = $"{metaSoldier.weapon.name} {metaSoldier.armour.name}";
            var buttonComponent = soldierTrans.GetComponentInChildren<ButtonHandler>();
            int soldierIndex = index;
            buttonComponent.action.AddListener(() => {
                ClickBenchedSoldier(soldierIndex);
            });
            index++;
        }
    }
}