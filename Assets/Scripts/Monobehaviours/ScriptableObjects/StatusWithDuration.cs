using UnityEngine;

[CreateAssetMenu(fileName = "Shocked", menuName = "Status Effects/Shocked", order = 0)]
public class StatusWithDuration : StatusEffect {
    
    public int duration;
    
    protected override void OnTick() {
        duration--;
        Debug.Log(duration);
        if (duration <= 0) Remove();
    }
}