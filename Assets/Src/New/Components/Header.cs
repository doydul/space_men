using UnityEngine;
using TMPro;

public class Header : MonoBehaviour {

    public static Header instance { get; private set; }

    void Awake() {
        instance = this;
        Hide();
    }
    
    public Icon weaponIcon;
    public Icon armourIcon;
    public TMP_Text infoText;

    public void Display(Soldier soldier) {
        weaponIcon.Enable();
        armourIcon.Enable();
        weaponIcon.Init(Weapon.Get(soldier.weaponName));
        armourIcon.Init(Armour.Get(soldier.armourName));
        var text = "";
        text += "HP: " + soldier.health + "/" + soldier.maxHealth + "\n";
        text += "Weapon: " + soldier.weaponName + "\n";
        text += "Armour: " + soldier.armourName;

        infoText.text = text;
    }

    public void Display(Alien alien) {
        weaponIcon.Disable();
        armourIcon.Disable();
        var text = "";
        text += "Type: " + alien.type + "\n";
        text += "HP: " + alien.health + "/" + alien.maxHealth + "\n";
        text += "Move Speed: " + alien.movement + "\n";
        text += "Damage: " + alien.damage + "\n";
        text += "Armour Penetration: " + alien.armourPen + "\n";

        infoText.text = text;
    }

    public void Display(Actor actor) {
        if (actor == null) return;
        Show();
        if (actor is Soldier) {
            Display(actor as Soldier);
        } else if (actor is Alien) {
            Display(actor as Alien);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}