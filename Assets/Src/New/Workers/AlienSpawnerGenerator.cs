using Data;
using System.Linq;
using System.Collections.Generic;

namespace Workers {
    
    public class AlienSpawnerGenerator {
        
        Data.Mission mission;
        ProfileWrapper[] wrappers;
        
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
                        remainingAliens = profile.groupSize,
                        spawnType = profile.spawnType
                    });
                    wrapper.cooldownTimer = profile.cooldown;
                }
            }
            return result.ToArray();
        }
        
        void InitWrappers() {
            wrappers = mission.spawnProfiles.Select(profile => {
                return new ProfileWrapper { profile = profile };
            }).ToArray();
        }
        
        class ProfileWrapper {
            public SpawnProfile profile;
            public int cooldownTimer;
        }
    }
}
