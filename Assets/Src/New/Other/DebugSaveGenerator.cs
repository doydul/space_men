public class DebugSaveGenerator {
    public static MetaGameSave Generate() {
        int soldierNum = 6;
        var items = new MetaItemSave[soldierNum * 2 + 3];
        var soldiers = new MetaSoldierSave[soldierNum];
        for (int i = 0; i < soldierNum; i++) {
            items[i * 2] = new MetaItemSave {
                uniqueId = i * 2,
                name = "Basic",
                type = MetaItemTypeSave.Armour
            };
            items[i * 2 + 1] = new MetaItemSave {
                uniqueId = i * 2 + 1,
                name = "Assault Rifle",
                type = MetaItemTypeSave.Weapon
            };
            soldiers[i] = new MetaSoldierSave {
                uniqueId = i,
                name = "Dave",
                armourId = i * 2,
                weaponId = i * 2 + 1
            };
        }
        items[items.Length - 3] = new MetaItemSave {
                uniqueId = items.Length - 3,
                name = "Grenade Launcher",
                type = MetaItemTypeSave.Weapon
            };
        items[items.Length - 2] = new MetaItemSave {
                uniqueId = items.Length - 2,
                name = "Grenade Launcher",
                type = MetaItemTypeSave.Weapon
            };
        items[items.Length - 1] = new MetaItemSave {
                uniqueId = items.Length - 1,
                name = "Plasma Rifle",
                type = MetaItemTypeSave.Weapon
            };
        return new MetaGameSave {
            credits = 1000,
            currentCampaign = "Default",
            currentMission = "First Mission",
            items = items,
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
            soldiers = soldiers,
            squadIds = new long[] {
                1, 2, 3, 4
            }
        };
    }
}