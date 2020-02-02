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
                    name = "Basic",
                    type = MetaItemTypeSave.Armour
                },
                new MetaItemSave {
                    uniqueId = 4,
                    name = "Basic",
                    type = MetaItemTypeSave.Armour
                },
                new MetaItemSave {
                    uniqueId = 5,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 6,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 7,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 8,
                    name = "Assault Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 9,
                    name = "Plasma Rifle",
                    type = MetaItemTypeSave.Weapon
                },
                new MetaItemSave {
                    uniqueId = 10,
                    name = "Grenade Launcher",
                    type = MetaItemTypeSave.Weapon
                }
            },
            blueprints = new MetaItemSave[] {
                new MetaItemSave {
                    name = "Default",
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
                    weaponId = 5
                },
                new MetaSoldierSave {
                    uniqueId = 2,
                    armourId = 2,
                    weaponId = 6
                },
                new MetaSoldierSave {
                    uniqueId = 3,
                    armourId = 3,
                    weaponId = 7
                },
                new MetaSoldierSave {
                    uniqueId = 4,
                    armourId = 4,
                    weaponId = 8
                }
            },
            squadIds = new long[] {
                1, 2, 3, 4
            }
        };
    }
}