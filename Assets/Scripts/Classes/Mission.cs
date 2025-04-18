using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Mission {
    
    public static Mission current => PlayerSave.current.mission;
    
    public static Mission Generate(PlayerSave playerSave) {
        var result = new Mission();
        
        result.enemyProfiles = EnemyProfileSet.Generate(playerSave);
        UnityEngine.Debug.Log("Mission generated:");
        UnityEngine.Debug.Log("Primaries:");
        foreach (var enemyProfile in result.enemyProfiles.primaries) {
            playerSave.alienUnlocks.Unlock(enemyProfile.name);
            UnityEngine.Debug.Log(enemyProfile.name);
        }
        UnityEngine.Debug.Log("Secondaries:");
        foreach (var enemyProfile in result.enemyProfiles.secondaries) {
            playerSave.alienUnlocks.Unlock(enemyProfile.name);
            UnityEngine.Debug.Log(enemyProfile.name);
        }
        
        result.objectives = Objectives.GenerateObjectiveList(playerSave);
        
        playerSave.mission = result;
        playerSave.levelSeed = Random.Range(int.MinValue, int.MaxValue);
        
        return result;
    }
    
    public bool finished { get; private set; }
    
    public EnemyProfileSet enemyProfiles;
    public List<ObjectiveData> objectives;
    
    public List<Objective> GetObjectives() {
        return objectives.Select(obj => obj.Dump()).ToList();
    }
    
    public void End() {
        finished = true;
        ModalPopup.instance.DisplayEOL();
    }
    
    public static void CheckGameOver() {
        if (PlayerSave.current.squad.GetMetaSoldiers().Count() <= 0) {
            AnimationManager.instance.StartCoroutine(NotificationPopup.PerformShow("mission failed", "", new BtnData("exit", () => {
                PlayerSave.current.Delete();
                SceneManager.LoadScene("MainMenu");
            })));
        }
    }
}