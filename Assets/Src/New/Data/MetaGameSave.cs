[System.Serializable]
public struct MetaGameSave {

    public MetaSoldierSave[] soldiers;
    public MetaItemSave[] items;
    public MetaItemSave[] blueprints;
    public int credits;
    public string currentCampaign;
    public string currentMission;
}