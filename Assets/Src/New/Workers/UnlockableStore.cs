using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Workers;

public class UnlockableStore : MonoBehaviour, IUnlockableStore {
    
    public UnlockableGraph graph;

    Dictionary<UnlockableType, Unlockable> data;

    void Awake() {
        data = new Dictionary<UnlockableType, Unlockable>();
        InitData();
        Debug.Log(GetUnlockable(UnlockableType.Stims).parent.type);
    }

    void InitData() {
        foreach (var unlockable in graph.GetUnlockables()) {
            UnlockableNode parent = unlockable.GetParent();
            data.Add(unlockable.type, new Unlockable(
                unlockable.type,
                parent == null ? null : data[parent.type]
            ));
        }
    }

    public Unlockable[] GetUnlockables() {
        return data.Values.ToArray();
    }

    public Unlockable GetUnlockable(UnlockableType type) {
        return data[type];
    }
}