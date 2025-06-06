using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionProfile_000", menuName = "Mission Enemy Profile", order = 1)]
public class MissionEnemyProfile : ScriptableObject {

    [System.Serializable]
    public class MissionEnemy {
        
        public bool randomized;
        public int threat;
        public EnemyProfile type;
        public int difficulty;
        public int subLevel;
    }
    
    public List<MissionEnemy> missionEnemies;
    
    public static MissionEnemyProfile Get(int index) {
        return Resources.Load<MissionEnemyProfile>($"Missions/MissionProfile_{index.ToString().PadLeft(3, '0')}");
    }
}
