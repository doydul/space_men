using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Mission {
    
    public class EnemyProfileWithThreat {
        public int threat;
        public EnemyProfile profile;
    }
    
    public static Mission current => PlayerSave.current.mission;
    
    public static Mission Generate(PlayerSave playerSave) {
        var result = new Mission();
        
        result.objectives = Objectives.GenerateObjectiveList(playerSave);
        result.LoadEnemyProfiles(playerSave);
        foreach (var prof in result.enemyProfiles) Debug.Log($"Enemy profile: {prof.profile.name}, {prof.threat}");
        
        playerSave.mission = result;
        playerSave.levelSeed = Random.Range(int.MinValue, int.MaxValue);
        
        return result;
    }
    
    public bool finished { get; private set; }
    
    public List<EnemyProfileWithThreat> enemyProfiles;
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
    
    void LoadEnemyProfiles(PlayerSave save) {
        var missionProfile = MissionEnemyProfile.Get(save.levelNumber);
        
        enemyProfiles = new();
        var primaryValue = save.GetValue(save.primaryMechanic);
        var secondaryValue = save.GetValue(save.secondaryMechanic);
        var secondaryRatio = 1 / Mathf.Floor((secondaryValue + primaryValue) / secondaryValue);
        
        int randomisedEnemiesCount = missionProfile.missionEnemies.Where(missionEnemy => missionEnemy.randomized).Count();
        int counter = 0;
        
        foreach (var missionEnemy in missionProfile.missionEnemies) {
            if (!missionEnemy.randomized) {
                enemyProfiles.Add(new EnemyProfileWithThreat { profile = missionEnemy.type, threat = missionEnemy.threat});
            } else {
                var tmpSave = PlayerSave.New();
                if ((float)counter / (float)randomisedEnemiesCount >= secondaryRatio) {
                    tmpSave.enemyGenerationValues[(int)save.secondaryMechanic] = 100;
                } else {
                    tmpSave.enemyGenerationValues[(int)save.primaryMechanic] = 100;
                }
                var ep = EnemyProfile.GetAll().Where(profile => profile.difficultyLevel == missionEnemy.difficulty &&
                    profile.subLevel == missionEnemy.subLevel &&
                    profile.Fits(tmpSave)
                ).Sample();
                enemyProfiles.Add(new EnemyProfileWithThreat { profile = ep, threat = missionEnemy.threat });
                counter++;
            }
        }
    }
}