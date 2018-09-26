using UnityEngine;
using System.Collections.Generic;

public interface ISpawnModule {

    bool finished { get; }

    int GetVirtualAliensCount();
}
