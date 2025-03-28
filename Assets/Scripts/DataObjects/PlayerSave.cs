using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave {
    
    public static PlayerSave current;
    
    public int slot;
    public int levelNumber;
    public int credits;
    public float _difficulty = -0.2f;
    public float difficulty => Mathf.Max(_difficulty, 0);
    public void IncreaseDifficulty(float amount) => _difficulty += amount;
    public MetaSquad squad;
    public List<MetaSoldier> bench = new();
    public Inventory inventory = new();
    public AlienUnlocks alienUnlocks = new();
    public Mission mission;

    public List<float> enemyGenerationValues = new List<float> { 0, 0, 0, 0 };
    public List<float> enemyGenerationVelocities = new List<float> { 0, 0, 0, 0 };

    public int groupishness => (int)enemyGenerationValues[0];
    public int armouredness => (int)enemyGenerationValues[1];
    public int quickness => (int)enemyGenerationValues[2];
    public int bigness => (int)enemyGenerationValues[3];
    public int groupishnessVelocity => (int)enemyGenerationVelocities[0];
    public int armourednessVelocity => (int)enemyGenerationVelocities[1];
    public int quicknessVelocity => (int)enemyGenerationVelocities[2];
    public int bignessVelocity => (int)enemyGenerationVelocities[3];
    
    private PlayerSave() {}

    public void Save() {
        string json = JsonUtility.ToJson(this);
        Debug.Log(Application.persistentDataPath);
        File.WriteAllText($"{Application.persistentDataPath}/game_{slot + 1}.save", json);
    }
    
    public void Delete() {
        File.Delete($"{Application.persistentDataPath}/game_{slot + 1}.save");
    }

    public static PlayerSave Load(int slot) {
        var filename = $"{Application.persistentDataPath}/game_{slot + 1}.save";
        if (!File.Exists(filename)) return null;
        string json = File.ReadAllText(filename);
        return JsonUtility.FromJson<PlayerSave>(json);
    }
    
    public static PlayerSave New() {
        var save = new PlayerSave();
        save.inventory.defaultWeapon = new InventoryItem { name = "SIKR-5", type = InventoryItem.Type.Weapon };
        save.inventory.defaultArmour = new InventoryItem { name = "Flak Vest", type = InventoryItem.Type.Armour };
        return save;
    }
}