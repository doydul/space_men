using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Icon : MonoBehaviour {
    
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
        var iconTransform = Instantiate(iconPrefab) as Transform;
        iconTransform.SetParent(transform, false);
    }
}