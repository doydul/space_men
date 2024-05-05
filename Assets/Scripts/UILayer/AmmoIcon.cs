using UnityEngine;
using UnityEngine.UI;

public class AmmoIcon : MonoBehaviour {
    
    public Image bulletIcon;
    public Color enabledColor;
    public Color disabledColor;

    public void Enable() => bulletIcon.color = enabledColor;
    public void Disable() => bulletIcon.color = disabledColor;
}