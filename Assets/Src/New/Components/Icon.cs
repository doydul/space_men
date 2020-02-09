using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Icon : MonoBehaviour {

    GameObject currentImage;
    
    public void Init(Weapon weapon) {
        Populate(weapon.icon);
    }

    public void Init(Armour armour) {
        Populate(armour.icon);
    }

    public void Init() {
        Populate(Resources.Load<Transform>("SoldierIcons/DefaultSoldierIcon"));
    }

    void Populate(Transform iconPrefab) {
        if (currentImage != null) Destroy(currentImage);
        var iconTransform = Instantiate(iconPrefab) as Transform;
        iconTransform.SetParent(transform, false);
        currentImage = iconTransform.gameObject;
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}