[System.Serializable]
public class MetaSoldier {

    public string id;
    public string name;
    public int exp;
    public int spentAbilityPoints;
    public InventoryItem armour;
    public InventoryItem weapon;
    public bool dead;
    public UnityEngine.Color tint;
    
    public void Dump(Soldier soldier) {
        soldier.id = id;
        soldier.armour = Armour.Get(armour.name);
        soldier.weapon = Weapon.Get(weapon.name);
        soldier.maxHealth = soldier.armour.maxHealth;
        soldier.health = soldier.armour.maxHealth;
        soldier.sightRange = soldier.armour.sightRange;
        soldier.SetTint(tint);

        foreach (var ability in soldier.weapon.abilities) ability.Attach(soldier);
        foreach (var ability in soldier.armour.abilities) ability.Attach(soldier);
    }
}