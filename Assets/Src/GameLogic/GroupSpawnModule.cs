using UnityEngine;
using System.Collections.Generic;
using System;

public class GroupSpawnModule : ISpawnModule {

    string type;
    bool expended;
    int count;

    public bool finished { get { return expended; } }

    public GroupSpawnModule(string type, int count) {
        if (count < 3) throw new Exception("Group must have at least 3 members");
        this.type = type;
        this.count = count;
    }

    public int GetVirtualAliensCount() {
        expended = true;
        return count;
    }

    public string GetAlienType() {
        return type;
    }
}
