using System.Linq;
using System;

using Data;

public class MissionStore : IMissionStore {
    
    public Data.Mission GetMission(string campaignName, string missionName) {
        foreach (var mission in Campaign.FromString(campaignName).missions) {
            if (mission.missionName == missionName) {
                return new Data.Mission {
                    missionName = missionName,
                    briefing = mission.briefing,
                    threatTimer = mission.threatTimer,
                    threatProfiles = mission.threatProfiles.Select(profile => new ThreatProfile {
                        spawnProfiles = profile.enemyProfiles.Select(enemyProfile => ProfileConverter(enemyProfile)).ToArray()
                    }).ToArray(),
                    spawnProfiles = mission.enemyProfiles.Select(profile => ProfileConverter(profile)).ToArray(),
                    rewards = mission.rewards.Select(reward => RewardConverter(reward)).ToArray(),
                    secondaryMissions = mission.secondaryMissions.Select(secMission => SecondaryMissionConverter(secMission)).ToArray()
                };
            }
        }
        return default(Data.Mission);
    }
    
    Data.SpawnProfile ProfileConverter(MissionEnemyProfile profile) {
        return new Data.SpawnProfile {
            alienType = profile.alienType,
            groupSizeMin = profile.groupSizeMin,
            groupSizeMax = profile.groupSizeMax,
            iterationsMin = profile.iterationsMin,
            iterationsMax = profile.iterationsMax,
            chance = profile.chance,
            cooldown = profile.cooldown
        };
    }

    IReward RewardConverter(MissionReward reward) {
        if (reward.type == MissionReward.Type.Armour) {
            return new ArmourReward(reward.itemName);
        } else if (reward.type == MissionReward.Type.Weapon) {
            return new WeaponReward(reward.itemName);
        } else {
            return new CreditReward(reward.credits);
        }
    }

    Data.SecondaryMission SecondaryMissionConverter(SecondaryMission secondaryMission) {
        return new Data.SecondaryMission {
            rewards = secondaryMission.rewards.Select(reward => RewardConverter(reward)).ToArray()
        };
    }
}
