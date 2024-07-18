using UnityEngine;
using UnityEngine.UI;

public class SoldierProfileIcon : MonoBehaviour {
    
    public Image image;
    
    public void DisplayMetaSoldier(MetaSoldier metaSoldier) {
        Debug.Log(metaSoldier);
        if (metaSoldier == null) {
            image.gameObject.SetActive(false);
        } else {
            image.gameObject.SetActive(true);
        }
    }
}