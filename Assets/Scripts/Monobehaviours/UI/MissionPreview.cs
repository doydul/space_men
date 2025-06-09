using UnityEngine;
using System.Linq;

public class MissionPreview : MonoBehaviour {
    
    public Transform alienIconPrototype;
    
    void Start() {
        foreach (var profile in Mission.current.enemyProfiles.Select(prof => prof.profile)) InstantiateIcon(profile, true);
        alienIconPrototype.gameObject.SetActive(false);
    }
    
    void InstantiateIcon(EnemyProfile profile, bool primary) {
        var iconTrans = Instantiate(alienIconPrototype, alienIconPrototype.parent);
        var icon = iconTrans.GetComponent<AlienIcon>();
        icon.SetAlien(AlienData.Get(profile.typeName), primary);
    }
}