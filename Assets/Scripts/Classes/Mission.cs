using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class Mission {
    
    public static Mission current => PlayerSave.current.mission;
    
    public static Mission Generate() {
        var result = new Mission();
        
        result.enemyProfiles = EnemyProfileSet.Generate(PlayerSave.current);
        UnityEngine.Debug.Log("Mission generated:");
        UnityEngine.Debug.Log("Primaries:");
        foreach (var enemyProfile in result.enemyProfiles.primaries) {
            PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
            UnityEngine.Debug.Log(enemyProfile.name);
        }
        UnityEngine.Debug.Log("Secondaries:");
        foreach (var enemyProfile in result.enemyProfiles.secondaries) {
            PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
            UnityEngine.Debug.Log(enemyProfile.name);
        }
        
        PlayerSave.current.mission = result;
        
        return result;
    }
    
    public bool finished { get; private set; }
    
    public EnemyProfileSet enemyProfiles;
    
    public void End() {
        finished = true;
        ModalPopup.instance.DisplayEOL();
    }
    
    public static void CheckGameOver() {
        if (PlayerSave.current.squad.GetMetaSoldiers().Count() <= 0) {
            NotificationPopup.Show("mission failed", "", new BtnData("exit", () => SceneManager.LoadScene("MainMenu")));
        }
    }
}