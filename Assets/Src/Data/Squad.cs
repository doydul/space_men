using System;
using System.Collections.Generic;

[Serializable]
public class Squad {
    
    public string name;
    public List<SoldierData> soldiers;
    public string currentCampaignName;
    public int currentMissionIndex;
    
    private static Squad instance;
    
    public static Squad active {
        get { return instance; }
    }
    
    public static Campaign currentCampaign {
        get {
            return Campaign.FromString(instance.currentCampaignName);
        }
    }
    
    public static Mission currentMission {
        get {
            return currentCampaign.missions[instance.currentMissionIndex];
        }
    }
    
    public Squad() {
        soldiers = new List<SoldierData>();
    }
    
    public static void SetActive(Squad activeSquad) {
        instance = activeSquad;
    }
    
    public static Squad GenerateDefault() {
        var result = new Squad();
        for (int i = 0; i < 4; i++) {
            result.soldiers.Add(SoldierData.GenerateDefault());
        }
        result.currentCampaignName = Campaign.DEFAULT;
        return result;
    }
}