using UnityEngine;
using UnityEngine.UI;

public class AlienIcon : MonoBehaviour {
    
    public GameObject primaryIndicator;
    
    AlienData alien;
    
    public void SetAlien(AlienData alien, bool primary) {
        primaryIndicator.SetActive(primary);
        this.alien = alien;
        Instantiate(Resources.Load<AlienImage>("Prefabs/AlienSprites/Images/" + alien.name + "Image"), transform);
    }
    
    public void Select() {
        AlienInfoCampaignComponent.instance.ShowAlien(alien);
    }
}