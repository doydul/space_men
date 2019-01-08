using UnityEngine;
using System.Collections.Generic;
using System;

public class TrickleSpawnModule : ISpawnModule {

    string type;
    int count;
    int spawnedCount;

    public bool finished { get { return spawnedCount >= count; } }

    public TrickleSpawnModule(string type, int count) {
        if (count < 3) throw new Exception("Spawn count must be at least 3");
        this.type = type;
        this.count = count;
    }

    public int GetVirtualAliensCount() {
        int numberToSpawn = 1;
        if (count - spawnedCount >= 2 && UnityEngine.Random.value < 0.3f) numberToSpawn = 2;
        spawnedCount += numberToSpawn;
        return numberToSpawn;
    }

    public string GetAlienType() {
        return type;
    }
}
