using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Squad {

    private static Squad instance;

    public static Squad active { get { return instance; } }
    public static List<SoldierData> activeSoldiers { get { return instance._activeSoldiers; } }
    public static List<SoldierData> reserveSoldiers { get { return instance._reserveSoldiers; } }
    public static List<InventoryItem> items { get { return instance._items; } }
    public static Campaign currentCampaign { get { return Campaign.FromString(instance.currentCampaignName); } }
    public static Mission currentMission {
        get {
            if (instance.currentMissionIndex < currentCampaign.missions.Length) {
                return currentCampaign.missions[instance.currentMissionIndex];
            } else {
                return null;
            }
        }
    }

    public string name;
    public List<SoldierData> _activeSoldiers;
    public List<SoldierData> _reserveSoldiers;
    public List<InventoryItem> _items;
    public string currentCampaignName;
    public int currentMissionIndex;

    public Squad() {
        _activeSoldiers = new List<SoldierData>();
        _reserveSoldiers = new List<SoldierData>();
        _items = new List<InventoryItem>();
    }

    public static SoldierData GetSoldier(int index) {
        if (activeSoldiers.Count <= index) return null;
        return activeSoldiers[index];
    }

    public static void SetActive(Squad activeSquad) {
        instance = activeSquad;
    }

    public static Squad GenerateDefault() {
        var result = new Squad();
        for (int i = 0; i < 3; i++) {
            result._activeSoldiers.Add(SoldierData.GenerateDefault());
        }

        var sol = new SoldierData();
        sol.armour = SoldierData.DEFAULT_ARMOUR;
        sol.weapon = "Grenade Launcher";
        result._reserveSoldiers.Add(sol);

        sol = new SoldierData();
        sol.armour = SoldierData.DEFAULT_ARMOUR;
        sol.weapon = SoldierData.DEFAULT_WEAPON;
        result._activeSoldiers.Add(sol);

        result._items.Add(new InventoryItem() { name = "Plasma Rifle", isWeapon = true });

        result.currentCampaignName = Campaign.DEFAULT;
        return result;
    }

    public static void IncrementMission() {
        active.currentMissionIndex++;
    }

    public static void ReplaceSoldier(int squadPosition, SoldierData reserveSoldier) {
        var activeMember = activeSoldiers[squadPosition];
        reserveSoldiers.Add(activeMember);
        reserveSoldiers.Remove(reserveSoldier);
        activeSoldiers[squadPosition] = reserveSoldier;
    }
}
