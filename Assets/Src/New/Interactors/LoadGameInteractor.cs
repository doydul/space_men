using Data;
using Workers;

namespace Interactors {
    
    public class LoadGameInteractor : Interactor<LoadGameOutput> {

        public IMetaGameStateStore metaGameStateStore { private get; set; }

        public void Interact(LoadGameInput input) {
            var output = new LoadGameOutput();
            
            if (metaGameStateStore.SaveExists(input.slotId)) {
                var save = metaGameStateStore.GetSave(input.slotId);
                MetaGameState.Load(input.slotId, save);
            } else {
                MetaGameState.Load(input.slotId, GenerateDefaultSave());
            }
            output.success = true;
            
            presenter.Present(output);
        }

        MetaGameSave GenerateDefaultSave() {
            return new MetaGameSave {
                credits = 0,
                currentCampaign = "Default",
                currentMission = "First Mission",
                items = new MetaItemSave[] {
                    new MetaItemSave {
                        uniqueId = 1,
                        name = "Default",
                        type = MetaItemTypeSave.Armour
                    },
                    new MetaItemSave {
                        uniqueId = 2,
                        name = "Default",
                        type = MetaItemTypeSave.Armour
                    },
                    new MetaItemSave {
                        uniqueId = 3,
                        name = "Default",
                        type = MetaItemTypeSave.Armour
                    },
                    new MetaItemSave {
                        uniqueId = 4,
                        name = "Default",
                        type = MetaItemTypeSave.Armour
                    },
                    new MetaItemSave {
                        uniqueId = 5,
                        name = "Assault Rifle",
                        type = MetaItemTypeSave.Weapon
                    },
                    new MetaItemSave {
                        uniqueId = 5,
                        name = "Assault Rifle",
                        type = MetaItemTypeSave.Weapon
                    },
                    new MetaItemSave {
                        uniqueId = 5,
                        name = "Assault Rifle",
                        type = MetaItemTypeSave.Weapon
                    },
                    new MetaItemSave {
                        uniqueId = 5,
                        name = "Assault Rifle",
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
                }
            };
        }
    }
}
