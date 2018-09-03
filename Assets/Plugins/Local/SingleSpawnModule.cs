using UnityEngine;
using System.Collections.Generic;

public class SingleSpawnModule : ISpawnModule {

    private bool expended;

    public bool finished { get { return expended; } }

    public int GetVirtualAliensCount() {
        expended = true;
        return 1;
    }
}
