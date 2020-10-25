using UnityEngine;
using System.Collections.Generic;

public class UnlockableNode : MonoBehaviour {
    
    public UnlockableType type;

    public UnlockableNode GetParent() {
        return transform.parent.GetComponent<UnlockableNode>();
    }
}