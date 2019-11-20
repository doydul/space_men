using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour {

    public TMP_Text infoText;
    public GameObject infoPanelGO;

    void Awake() {
        Close();
    }

    void Open() {
        infoPanelGO.SetActive(true);
    }

    public void Close() {
        infoPanelGO.SetActive(false);
    }

    public void Display(Soldier soldier) {
        Open();
        var text = "";
        text += "HP: " + soldier.health + "/" + soldier.maxHealth + "\n";
        text += "Weapon: " + soldier.weaponName;

        infoText.text = text;
    }

    public void Display(Alien alien) {
        Open();
        var text = "";
        text += "Type: " + alien.type + "\n";
        text += "HP: " + alien.health + "/" + alien.maxHealth + "\n";
        text += "Move Speed: " + alien.movement + "\n";
        text += "Damage: " + alien.damage + "\n";
        text += "Armour Penetration: " + alien.armourPen + "\n";

        infoText.text = text;
    }

    public void Display(Actor actor) {
        if (actor is Soldier) {
            Display(actor as Soldier);
        } else if (actor is Alien) {
            Display(actor as Alien);
        }
    }
}