using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave {
    
    public static PlayerSave current;

    public List<InventoryItem> items = new();
    public MetaSquad squad;

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