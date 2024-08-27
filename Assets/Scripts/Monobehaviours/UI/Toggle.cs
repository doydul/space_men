using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Toggle : MonoBehaviour {
    
    public UnityEvent<bool> toggleAction;
    
    public Sprite onSprite;
    public Sprite offSprite;
    
    public Image image;
    
    public bool toggled { get; private set; }
    
    public void Flip() => SetState(!toggled);
    
    public void SetState(bool state) {
        toggled = state;
        if (toggled) image.sprite = onSprite;
        else image.sprite = offSprite;
        if (toggleAction != null) toggleAction.Invoke(toggled);
    }
}