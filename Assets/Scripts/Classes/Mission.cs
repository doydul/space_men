public class Mission {
    
    public static Mission current;
    
    public static Mission Generate() {
        var result = new Mission();
        current = result;
        
        result.enemyProfiles = EnemyProfileSet.Generate(PlayerSave.current);
        foreach (var enemyProfile in result.enemyProfiles.primaries) PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
        foreach (var enemyProfile in result.enemyProfiles.secondaries) PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
        
        return result;
    }
    
    public EnemyProfileSet enemyProfiles;
}