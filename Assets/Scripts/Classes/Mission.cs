[System.Serializable]
public class Mission {
    
    public static Mission current => PlayerSave.current.mission;
    
    public static Mission Generate() {
        var result = new Mission();
        
        result.enemyProfiles = EnemyProfileSet.Generate(PlayerSave.current);
        foreach (var enemyProfile in result.enemyProfiles.primaries) PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
        foreach (var enemyProfile in result.enemyProfiles.secondaries) PlayerSave.current.alienUnlocks.Unlock(enemyProfile.name);
        
        PlayerSave.current.mission = result;
        
        return result;
    }
    
    public bool finished { get; private set; }
    
    public EnemyProfileSet enemyProfiles;
    
    public void End() {
        finished = true;
        ModalPopup.instance.DisplayEOL();
    }
}