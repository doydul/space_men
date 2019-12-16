namespace Data {
  
    public struct Mission {

        public string missionName;
        public string briefing;
        public SpawnProfile[] spawnProfiles;
        public IReward[] rewards;
        public SecondaryMission[] secondaryMissions;
    }

    public struct SecondaryMission {

        public IReward[] rewards;
    }
}
