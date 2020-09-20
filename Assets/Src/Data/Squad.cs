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
    public static List<ItemBlueprint> blueprints { get { return instance._blueprints; } }
    public static Campaign currentCampaign { get { return Campaign.FromString(instance.currentCampaignName); } }
    public static int credits { get { return instance._credits; } }
    public static Mission currentMission {
        get {
            return null;
        }
    }

    public string name;
    public List<SoldierData> _activeSoldiers;
    public List<SoldierData> _reserveSoldiers;
    public List<InventoryItem> _items;
    public List<ItemBlueprint> _blueprints;
    public string currentCampaignName;
    public int currentMissionIndex;

    public int _credits;

    public Squad() {
        _activeSoldiers = new List<SoldierData>();
        _reserveSoldiers = new List<SoldierData>();
        _items = new List<InventoryItem>();
        _blueprints = new List<ItemBlueprint>();
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
        // result._credits = 1000;
        for (int i = 0; i < 2; i++) {
            result._activeSoldiers.Add(SoldierData.GenerateDefault());
        }

        // var sol = new SoldierData();
        // sol.armour = SoldierData.DEFAULT_ARMOUR;
        // sol.weapon = "Grenade Launcher";
        // result._activeSoldiers.Add(sol);

        //  sol = new SoldierData();
        // sol.armour = SoldierData.DEFAULT_ARMOUR;
        // sol.weapon = "Plasma Rifle";
        // result._activeSoldiers.Add(sol);

        // sol = new SoldierData();
        // sol.armour = SoldierData.DEFAULT_ARMOUR;
        // sol.weapon = SoldierData.DEFAULT_WEAPON;
        // result._reserveSoldiers.Add(sol);

        // result._items.Add(new InventoryItem() {
        //     name = "Assault Rifle",
        //     isWeapon = true
        // });

        // result._blueprints.Add(new ItemBlueprint() { item = new InventoryItem() {
        //     name = "Plasma Rifle",
        //     isWeapon = true
        // }});

        result.currentCampaignName = Campaign.DEFAULT;
        return result;
    }

    public static void ChangeCredits(int amount) {
        instance._credits += amount;
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
