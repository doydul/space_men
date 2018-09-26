using UnityEngine;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour {

    public string sceneName;
    public string missionName;
    public MissionEnemyProfile[] enemyProfiles;

    [TextArea(15,20)]
    public string briefing;
}
