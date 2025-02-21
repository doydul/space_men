using UnityEngine;
using UnityEngine.UI;

public class AlienIcon : MonoBehaviour {
    
    AlienData alien;
    
    public void SetAlien(AlienData alien) {
        this.alien = alien;
        Instantiate(Resources.Load<AlienImage>("Prefabs/AlienSprites/Images/" + alien.name + "Image"), transform);
    }
    
    public void Select() {
        AlienInfoCampaignComponent.instance.ShowAlien(alien);
    }
}