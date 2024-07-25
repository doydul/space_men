using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave {
    
    public static PlayerSave current;

    public int levelNumber;
    public int credits;
    public float difficulty;
    public MetaSquad squad;
    public List<MetaSoldier> bench = new();
    public Inventory inventory = new();
    public AlienUnlocks alienUnlocks = new();
    public MapInstantiator.Blueprint mapBlueprint;

    public List<float> enemyGenerationValues = new List<float> { 15, 15, 15, 15 };
    public List<float> enemyGenerationVelocities = new List<float> { 0, 0, 0, 0 };

    public int groupishness => (int)enemyGenerationValues[0];
    public int armouredness => (int)enemyGenerationValues[1];
    public int quickness => (int)enemyGenerationValues[2];
    public int bigness => (int)enemyGenerationValues[3];
    public int groupishnessVelocity => (int)enemyGenerationVelocities[0];
    public int armourednessVelocity => (int)enemyGenerationVelocities[1];
    public int quicknessVelocity => (int)enemyGenerationVelocities[2];
    public int bignessVelocity => (int)enemyGenerationVelocities[3];

    public void Save(int slot) {
        string json = UnityEngine.JsonUtility.ToJson(this);
        Debug.Log(Application.persistentDataPath);
        File.WriteAllText($"{Application.persistentDataPath}/game_{slot + 1}.save", json);
    }

    public static PlayerSave Load(int slot) {
        string json = File.ReadAllText($"{Application.persistentDataPath}/game_{slot + 1}.save");
        return JsonUtility.FromJson<PlayerSave>(json);
    }
}