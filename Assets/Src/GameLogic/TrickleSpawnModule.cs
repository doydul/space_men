using UnityEngine;
using System.Collections.Generic;
using System;

public class TrickleSpawnModule : ISpawnModule {

    private int count;
    private int spawnedCount;

    public bool finished { get { return spawnedCount >= count; } }

    public TrickleSpawnModule(int count) {
        if (count < 3) throw new Exception("Spawn count must be at least 3");
        this.count = count;
    }

    public int GetVirtualAliensCount() {
        int numberToSpawn = 1;
        if (count - spawnedCount >= 2 && UnityEngine.Random.value < 0.3f) numberToSpawn = 2;
        spawnedCount += numberToSpawn;
        return numberToSpawn;
    }
}
