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

    Actor displayedActor;

    void Update() {
        if (displayedActor != null) {
            infoText.text = DescriptionFor(displayedActor);
        }
    }

    public void Display(Soldier soldier) {
        weaponIcon.Enable();
        armourIcon.Enable();
        weaponIcon.Init(Weapon.Get(soldier.weaponName));
        armourIcon.Init(Armour.Get(soldier.armourName));
    }

    public void Display(Alien alien) {
        weaponIcon.Disable();
        armourIcon.Disable();
    }

    public void Display(Actor actor) {
        if (actor == null) return;
        displayedActor = actor;
        Show();
        if (actor is Soldier) {
            Display(actor as Soldier);
        } else if (actor is Alien) {
            Display(actor as Alien);
        }
    }

    public string DescriptionFor(Actor actor) {
        var text = "";
        if (actor is Alien) {
            var alien = actor as Alien;
            text += "Type: " + alien.type + "\n";
            text += "HP: " + alien.health + "/" + alien.maxHealth + "\n";
            text += "Move Speed: " + alien.movement + "\n";
            text += "Damage: " + alien.damage + "\n";
            text += "Armour(%): " + alien.armour + "\n";
        } else if (actor is Soldier) {
            var soldier = actor as Soldier;
            text += "HP: " + soldier.health + "/" + soldier.maxHealth + "ammo: " + soldier.ammo + "/" + soldier.maxAmmo + "\n";
            text += "Weapon: " + soldier.weaponName + "\n";
            text += "Armour: " + soldier.armourName;
        }
        return text;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        displayedActor = null;
        gameObject.SetActive(false);
    }
}