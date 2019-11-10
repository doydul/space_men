using UnityEngine;

public class Controller : MonoBehaviour {

    protected bool disabled;

    public void Enable() {
        disabled = false;
    }

    public void Disable() {
        disabled = true;
    }
}
