public class MetaSoldier {

    public long uniqueId { get; set; }
    public string name { get; set; }
    public MetaArmour armour { get; set; }
    public MetaWeapon weapon { get; set; }
    public int exp { get; set; }
    public int spentAbilityPoints { get; set; }
    public UnlockableType[] unlockedAbilities { get; set; }
}