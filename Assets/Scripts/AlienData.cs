using UnityEngine;

[CreateAssetMenu(fileName = "Alien", menuName = "Alien", order = 1)]
public class AlienData : ScriptableObject {

    public int maxHealth;
    public int armour;
    public int accModifier;
    public int damage;
    public int movement;

    public void Dump(Alien target) {
        target.type = name;
        target.maxHealth = maxHealth;
        target.health = maxHealth;
        target.armour = armour;
        target.accModifier = accModifier;
        target.damage = damage;
        target.movement = movement;
        target.sensoryRange = 7;
    }
}
