using UnityEngine;

public class Mission : MonoBehaviour {

    public string missionName;
    public MissionEnemyProfile[] enemyProfiles;
    public int threatTimer;
    public MissionThreatProfile[] threatProfiles;
    public MissionReward[] rewards;
    public SecondaryMission[] secondaryMissions;

    [TextArea(15,20)]
    public string briefing;
}
