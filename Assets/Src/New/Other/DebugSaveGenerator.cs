public class DebugSaveGenerator {
    public static MetaGameSave Generate() {
        return new MetaGameSave {
            credits = 0,
            currentCampaign = "Default",
            currentMission = "First Mission",
            items = new MetaItemSave[] {
                new MetaItemSave {
                    uniqueId = 1,
                    name = "Basic",
                    type = MetaItemTypeSave.Armour
                },
                new MetaItemSave {
                    uniqueId = 2,
                    name = "Basic",
                    type = MetaItemTypeSave.Armour
                },
                new MetaItemSave {
                    uniqueId = 3,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 4,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                }
            },
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
            soldiers = new MetaSoldierSave[] {
                new MetaSoldierSave {
                    uniqueId = 1,
                    armourId = 1,
                    weaponId = 3
                },
                new MetaSoldierSave {
                    uniqueId = 2,
                    armourId = 2,
                    weaponId = 4
                }
            },
            squadIds = new long[] {
                1, 2
            }
        };
    }
}