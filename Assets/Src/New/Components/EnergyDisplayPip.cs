using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplayPip : MonoBehaviour {

    public Image image;
    public Sprite fullSprite;
    public Sprite emptySprite;

    public bool full { get; private set; }
    
    public void Fill() {
        full = true;
        image.sprite = fullSprite;
    }

    public void Drain() {
        full = false;
        image.sprite = emptySprite;
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}