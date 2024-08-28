using UnityEngine;
using UnityEngine.UI;

public class AlienIcon : MonoBehaviour {
    
    public Image alienImage;
    
    AlienData alien;
    
    public void SetAlien(AlienData alien) {
        this.alien = alien;
        alienImage.sprite = alien.sprite;
    }
    
    public void Select() {
        AlienInfoCampaignComponent.instance.ShowAlien(alien);
    }
}