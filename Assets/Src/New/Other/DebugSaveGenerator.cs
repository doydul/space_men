using System.Collections.Generic;
using DataTypes;

public class DebugSaveGenerator {
    public static MetaGameSave Generate() {
        var soldiers = new List<MetaSoldierSave>();
        var items = new List<MetaItemSave>();
        var dict = new IDDictionary<object>();
        GenerateSoldier(dict, soldiers, items, "Basic", "Assault Rifle");
        GenerateSoldier(dict, soldiers, items, "Basic", "Grenade Launcher");
        GenerateSoldier(dict, soldiers, items, "Medium", "Slugger");
        GenerateSoldier(dict, soldiers, items, "Heavy", "Needler");
        GenerateSoldier(dict, soldiers, items, "Basic", "Assault Rifle");

        return new MetaGameSave {
            credits = 0,
            currentCampaign = "Default",
            currentMission = "First Mission",
            items = items.ToArray(),
            blueprints = new MetaItemSave[] {
                new MetaItemSave {
                    name = "Basic",
                    type = MetaItemTypeSave.Armour
                },
                new MetaItemSave {
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                }
            },
            soldiers = soldiers.ToArray(),
            squadIds = new long[] {
                soldiers[0].uniqueId,
                soldiers[1].uniqueId,
                soldiers[2].uniqueId,
                soldiers[3].uniqueId
            }
        };
    }

    static void GenerateSoldier(IDDictionary<object> dict, List<MetaSoldierSave> soldiers, List<MetaItemSave> items, string armourName, string weaponName) {
        var arm = new MetaItemSave {
            uniqueId = dict.GenerateUniqueId(),
            name = armourName,
            type = MetaItemTypeSave.Armour
        };
        var wep = new MetaItemSave {
            uniqueId = dict.GenerateUniqueId(),
            name = weaponName,
            type = MetaItemTypeSave.Weapon
        };
        var sol = new MetaSoldierSave {
            uniqueId = dict.GenerateUniqueId(),
            name = "Dave",
            armourId = arm.uniqueId,
            weaponId = wep.uniqueId
        };
        items.Add(wep);
        items.Add(arm);
        soldiers.Add(sol);
    }
}