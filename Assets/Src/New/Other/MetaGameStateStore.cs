using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MetaGameStateStore : IMetaGameStateStore {

    public MetaGameSave GetSave(int slot) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(PathFor(slot), FileMode.Open);
        var save = (MetaGameSave)bf.Deserialize(file);
        file.Close();
        return save;
    }

    public bool SaveExists(int slot) {
        return File.Exists(PathFor(slot));
    }

    public void Save(int slot, MetaGameSave save) {
        var bf = new BinaryFormatter();
        var file = File.Create(PathFor(slot));
        bf.Serialize(file, save);
        file.Close();
    }

    string PathFor(int slot) {
        return Application.persistentDataPath + "/gamesave_" + slot + ".save";
    }
}