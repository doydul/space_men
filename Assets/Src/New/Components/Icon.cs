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

    public void Init(int credits) {
        var icon = Populate(Resources.Load<Transform>("Weapons/Icons/CreditsIcon"));
        var script = icon.GetComponent<CreditsIcon>();
        script.SetCredits(credits);
    }

    public void Init() {
        Populate(Resources.Load<Transform>("SoldierIcons/DefaultSoldierIcon"));
    }

    Transform Populate(Transform iconPrefab) {
        if (currentImage != null) Destroy(currentImage);
        var iconTransform = Instantiate(iconPrefab) as Transform;
        iconTransform.SetParent(transform, false);
        currentImage = iconTransform.gameObject;
        return iconTransform;
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}