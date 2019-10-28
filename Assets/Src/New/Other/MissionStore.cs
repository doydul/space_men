using System.Linq;
using System;

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
            alienType = (Data.AlienType)Enum.Parse(typeof(Data.AlienType), profile.alienType, true),
            groupSize = profile.groupSize,
            spawnType = (Data.AlienSpawnType)profile.spawnType,
            chance = profile.chance,
            cooldown = profile.cooldown
        };
    }
}
