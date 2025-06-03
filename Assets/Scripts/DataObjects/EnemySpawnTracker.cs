public class EnemySpawnTracker : IWeighted {
    
    public EnemyProfile profile;
    public int startingThreat;
    public int spawningThreat;
    public int weight;
    public int Weight => weight;
    public int groupSize;
    
    public int initialThreat => startingThreat - spawningThreat;
    public int threatCost => profile.threat;
    public int totalThreatCost => threatCost * groupSize;
    public string type => profile.typeName;
}