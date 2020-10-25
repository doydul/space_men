using UnityEngine;
using System.Collections.Generic;

public class UnlockableGraph : MonoBehaviour {
    
    public IEnumerable<UnlockableNode> GetUnlockables() {
        foreach (var node in GetComponentsInChildren<UnlockableNode>()) {
            yield return node;
        }
    }
}