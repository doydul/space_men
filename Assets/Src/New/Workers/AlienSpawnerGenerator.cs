using Data;
using System.Linq;
using System.Collections.Generic;

namespace Workers {
    
    public class AlienSpawnerGenerator {
        
        Data.Mission mission;
        List<ProfileWrapper> wrappers;
        int threatIndex;
        
        public AlienSpawnerGenerator(Data.Mission mission) {
            this.mission = mission;
            InitWrappers();
        }
        
        public AlienSpawner[] Iterate() {
            var result = new List<AlienSpawner>();
            var random = new System.Random();
            foreach (var wrapper in wrappers) {
                var profile = wrapper.profile;
                wrapper.cooldownTimer--;
                if (wrapper.cooldownTimer <= 0 && random.NextDouble() < profile.chance) {
                    result.Add(new AlienSpawner {
                        alienType = profile.alienType,
                        remainingIterations = profile.iterations,
                        groupSize = profile.groupSize
                    });
                    wrapper.cooldownTimer = profile.cooldown;
                }
            }
            return result.ToArray();
        }

        public void EscalateThreat() {
            threatIndex++;
            if (threatIndex <= mission.threatProfiles.Length) {
                wrappers.AddRange(mission.threatProfiles[threatIndex - 1].spawnProfiles.Select(profile => {
                    return new ProfileWrapper { profile = profile };
                }));
            }
        }
        
        void InitWrappers() {
            wrappers = mission.spawnProfiles.Select(profile => {
                return new ProfileWrapper { profile = profile };
            }).ToList();
        }
        
        class ProfileWrapper {
            public SpawnProfile profile;
            public int cooldownTimer;
        }
    }
}
