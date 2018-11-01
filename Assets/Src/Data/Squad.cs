using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Squad {

    public string name;
    public List<SoldierData> activeSoldiers;
    public List<SoldierData> reserveSoldiers;
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
            if (instance.currentMissionIndex < currentCampaign.missions.Length) {
                return currentCampaign.missions[instance.currentMissionIndex];
            } else {
                return null;
            }
        }
    }

    public Squad() {
        activeSoldiers = new List<SoldierData>();
        reserveSoldiers = new List<SoldierData>();
    }

    public static SoldierData GetSoldier(int index) {
        if (instance.activeSoldiers.Count <= index) return null;
        return instance.activeSoldiers[index];
    }

    public static void SetActive(Squad activeSquad) {
        instance = activeSquad;
    }

    public static Squad GenerateDefault() {
        var result = new Squad();
        for (int i = 0; i < 3; i++) {
            result.activeSoldiers.Add(SoldierData.GenerateDefault());
        }

        var sol = new SoldierData();
        sol.armour = SoldierData.DEFAULT_ARMOUR;
        sol.weapon = "Grenade Launcher";
        result.activeSoldiers.Add(sol);

        result.currentCampaignName = Campaign.DEFAULT;
        return result;
    }

    public static void IncrementMission() {
        active.currentMissionIndex++;
    }
}
