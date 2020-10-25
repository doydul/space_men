using DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace Workers {

    public class MetaGameState {

        public static MetaGameState instance { get; private set; }

        public static void Load(int slot, MetaGameSave save) {
            if (instance != null) throw new System.Exception("Game already loaded!");
            instance = new MetaGameState();
            var itemsAlreadyUsed = new HashSet<long>();
            foreach (var soldierSave in save.soldiers) {
                itemsAlreadyUsed.Add(soldierSave.armourId);
                itemsAlreadyUsed.Add(soldierSave.weaponId);
                var armourSave = save.items.First(item => item.uniqueId == soldierSave.armourId);
                var weaponSave = save.items.First(item => item.uniqueId == soldierSave.weaponId);
                var metaArmour = new MetaArmour {
                    uniqueId = armourSave.uniqueId,
                    name = armourSave.name
                };
                var metaWeapon = new MetaWeapon {
                    uniqueId = weaponSave.uniqueId,
                    name = weaponSave.name
                };
                instance.metaSoldiers.Add(new MetaSoldier {
                    uniqueId = soldierSave.uniqueId,
                    name = soldierSave.name,
                    armour = metaArmour,
                    weapon = metaWeapon,
                    exp = soldierSave.exp,
                    spentAbilityPoints = soldierSave.spentAbilityPoints,
                    unlockedAbilities = soldierSave.unlockedAbilities
                });
            }
            foreach (var itemSave in save.items) {
                MetaItem metaItem;
                if (itemSave.type == MetaItemTypeSave.Armour) {
                    metaItem = new MetaArmour {
                        uniqueId = itemSave.uniqueId,
                        name = itemSave.name
                    };
                } else {
                    metaItem = new MetaWeapon {
                        uniqueId = itemSave.uniqueId,
                        name = itemSave.name
                    };
                }
                instance.metaItems.Add(metaItem);
                if (!itemsAlreadyUsed.Contains(itemSave.uniqueId)) instance.metaItems.MoveItemToInventory(metaItem.uniqueId);
            }
            foreach (var itemSave in save.blueprints) {
                MetaItem metaItem;
                if (itemSave.type == MetaItemTypeSave.Armour) {
                    metaItem = new MetaArmour {
                        name = itemSave.name
                    };
                } else {
                    metaItem = new MetaWeapon {
                        name = itemSave.name
                    };
                }
                instance.metaItems.AddBlueprint(metaItem);
            }
            for (int i = 0; i < save.squadIds.Length; i++) {
                instance.metaSoldiers.UpdateSquadRoster(save.squadIds[i], i);
            }
            instance.credits.Add(save.credits);
            instance.currentCampaign = save.currentCampaign;
            instance.currentMission = save.currentMission;
            instance.saveSlot = slot;
        }

        public static void Unload() {
            instance = null;
        }

        public MetaGameState() {
            metaSoldiers = new MetaSoldiers();
            metaItems = new MetaItems();
            credits = new Credits(0);
        }

        public int saveSlot { get; private set; }
        public MetaSoldiers metaSoldiers { get; private set; }
        public MetaItems metaItems { get; private set; }
        public Credits credits { get; private set; }
        public string currentCampaign { get; private set; }
        public string currentMission { get; set; }
    }
}
