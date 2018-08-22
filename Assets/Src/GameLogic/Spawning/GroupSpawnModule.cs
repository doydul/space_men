using UnityEngine;
using System.Collections.Generic;
using System;

public class GroupSpawnModule : ISpawnModule {

    private bool expended;
    private int count;

    public bool finished { get { return expended; } }

    public GroupSpawnModule(int count) {
        if (count < 3) throw new Exception("Group must have at least 3 members");
        this.count = count;
    }

    public int GetVirtualAliensCount() {
        expended = true;
        return count;
    }
}
