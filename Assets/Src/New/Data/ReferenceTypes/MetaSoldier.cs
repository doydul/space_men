public class MetaSoldier {

    public string id { get; set; }
    public string name { get; set; }
    public int exp { get; set; }
    public int spentAbilityPoints { get; set; }

    // remove these at some point
    public long uniqueId { get; set; }
    public MetaArmour armour { get; set; }
    public MetaWeapon weapon { get; set; }
    public UnlockableType[] unlockedAbilities { get; set; }
    //
}