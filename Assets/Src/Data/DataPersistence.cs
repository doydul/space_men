using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class DataPersistence {
    
    public const string DATA_PATH = "/saves.dat";
    
    public static List<Squad> squads;
    
    public static void Save() {
        File.WriteAllText(Application.persistentDataPath + DATA_PATH, JsonUtility.ToJson(new SquadCollection(squads)));
    }
    
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + DATA_PATH)) {
            var reader = new StreamReader(Application.persistentDataPath + DATA_PATH);
            squads = JsonUtility.FromJson<SquadCollection>(reader.ReadToEnd()).list;
            reader.Close();
        } else {
            squads = new List<Squad>();
        }
    }
    
    [Serializable]
    private class SquadCollection {
        public List<Squad> list;
        public SquadCollection(List<Squad> list) {
            this.list = list;
        }
    }
}