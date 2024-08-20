using UnityEngine;
using UnityEngine.UI;

public class AlienIcon : MonoBehaviour {
    
    public Image alienImage;
    
    public void SetAlien(AlienData alien) {
        alienImage.sprite = alien.sprite;
    }
}