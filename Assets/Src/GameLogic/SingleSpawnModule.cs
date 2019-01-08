using UnityEngine;
using System.Collections.Generic;

public class SingleSpawnModule : ISpawnModule {

    string type;
    bool expended;

    public bool finished { get { return expended; } }

    public SingleSpawnModule(string type) {
        this.type = type;
    }

    public int GetVirtualAliensCount() {
        expended = true;
        return 1;
    }

    public string GetAlienType() {
        return type;
    }
}
