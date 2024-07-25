using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBed : MonoBehaviour {
    
    public bool useTestMap;
    public float difficultyLevel;
    
    public int groupishness;
    public int armouredness;
    public int quickness;
    public int bigness;
    
    public MapInstantiator.Blueprint mapBlueprint;
    
    public string[] weapons;
    public string[] armour;
    
    public string[] unlockedAliens;
    
    void Start() {
        var save = new PlayerSave();
        PlayerSave.current = save;
        save.difficulty = difficultyLevel;
        save.mapBlueprint = mapBlueprint;

        save.squad = GenerateSquad();
        SetEnemyProfileValues();
        
        foreach (var alienName in unlockedAliens) PlayerSave.current.alienUnlocks.Unlock(alienName);
        
        Debug.Log("Loading Mission scene...");
        if (useTestMap) {
            MapInstantiator.skipGenerate = true;
            SceneManager.LoadScene("MapTest");
        } else {
            SceneManager.LoadScene("Mission");
        }
    }
    
    MetaSquad GenerateSquad() {
        var result = new MetaSquad();
        for (int i = 0; i < weapons.Length; i++) {
            result.AddMetaSoldier(new MetaSoldier() {
                name = "John Doe",
                armour = new InventoryItem() {
                    name = armour[i],
                    type = InventoryItem.Type.Armour
                },
                weapon = new InventoryItem() {
                    name = weapons[i],
                    type = InventoryItem.Type.Weapon
                }
            });
        }
        return result;
    }
    
    void SetEnemyProfileValues() {
        PlayerSave.current.enemyGenerationValues[0] = groupishness;
        PlayerSave.current.enemyGenerationValues[1] = armouredness;
        PlayerSave.current.enemyGenerationValues[2] = quickness;
        PlayerSave.current.enemyGenerationValues[3] = bigness;
    }
}