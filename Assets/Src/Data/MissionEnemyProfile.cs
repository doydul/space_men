[System.Serializable]
public class MissionEnemyProfile {

    public string alienType;
    public int groupSize;
    public AlienSpawnType spawnType;
    public float chance;
    public int cooldown;
}

public enum AlienSpawnType {
    Group,
    Trickle
}
