using System.Linq;

public class MissionStore : IMissionStore {
    
    public Data.Mission GetMission(string campaignName, string missionName) {
        foreach (var mission in Campaign.FromString(campaignName).missions) {
            if (mission.missionName == missionName) {
                return new Data.Mission {
                    missionName = missionName,
                    briefing = mission.briefing,
                    spawnProfiles = mission.enemyProfiles.Select(profile => ProfileConverter(profile)).ToArray()
                };
            }
        }
        return default(Data.Mission);
    }
    
    Data.SpawnProfile ProfileConverter(MissionEnemyProfile profile) {
        return new Data.SpawnProfile {
            alienType = profile.alienType,
            groupSize = profile.groupSize,
            spawnType = (Data.AlienSpawnType)profile.spawnType,
            chance = profile.chance,
            cooldown = profile.cooldown
        };
    }
}
