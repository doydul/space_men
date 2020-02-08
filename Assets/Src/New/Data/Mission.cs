namespace Data {
  
    public struct Mission {

        public string missionName;
        public string briefing;
        public SpawnProfile[] spawnProfiles;
        public int threatTimer;
        public ThreatProfile[] threatProfiles;
        public IReward[] rewards;
        public SecondaryMission[] secondaryMissions;
    }

    public struct SecondaryMission {

        public IReward[] rewards;
    }

    public struct ThreatProfile {

        public SpawnProfile[] spawnProfiles;
    }
}
