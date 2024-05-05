using UnityEngine;

public class AmmoGauge : MonoBehaviour {
    
    public static AmmoGauge instance;

    public GameObject ammoIconPrototype;

    void Awake() {
        instance = this;
        ammoIconPrototype.SetActive(false);
    }

    public void DisplayAmmo(int max, int current) {
        ClearAmmo();
        for (int i = 0; i < max; i++) {
            var newIcon = Instantiate(ammoIconPrototype, ammoIconPrototype.transform.parent) as GameObject;
            newIcon.SetActive(true);
            var ammoIcon = newIcon.GetComponent<AmmoIcon>();
            if (i >= current) {
                ammoIcon.Disable();
            } else {
                ammoIcon.Enable();
            }
        }
    }

    public void ClearAmmo() {
        var parent = ammoIconPrototype.transform.parent;
        for (int i = 1; i < parent.childCount; i++) {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}