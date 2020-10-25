[System.Serializable]
public struct MetaSoldierSave {

    public long uniqueId;
    public string name;
    public long armourId;
    public long weaponId;
    public int exp;
    public int spentAbilityPoints;
    public UnlockableType[] unlockedAbilities;
}