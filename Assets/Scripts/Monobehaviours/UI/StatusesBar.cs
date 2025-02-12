using UnityEngine;
using System.Collections.Generic;

public class StatusesBar : MonoBehaviour {
    
    public StatusIcon statusIconPrototype;
    public float iconSeperation;
    
    void Awake() {
        statusIconPrototype.gameObject.SetActive(false);
    }
    
    public void DisplayStatuses(IEnumerable<StatusEffect> statusEffects) {
        transform.DestroyChildren(1);
        int i = 0;
        foreach (var status in statusEffects) {
            var icon = Instantiate(statusIconPrototype, transform);
            icon.gameObject.SetActive(true);
            icon.transform.position = statusIconPrototype.transform.position + new Vector3(i * iconSeperation, 0, 0);
            icon.DisplaySpriteFor(status);
            i++;
        }
    }
}