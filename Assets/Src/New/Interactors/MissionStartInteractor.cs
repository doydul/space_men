using Data;
using Workers;

namespace Interactors {
    
    public class MissionStartInteractor : Interactor<MissionStartOutput> {

        public void Interact(MissionStartInput input) {
            var output = new MissionStartOutput();
            
            var squad = GenerateDefaultSquad();
            output.soldiers = new SoldierWithIndex[squad.Length];
            
            foreach (var soldier in squad) {
                var index = gameState.AddSoldier(soldier);
                output.soldiers[index] = new SoldierWithIndex {
                    soldier = soldier,
                    index = index
                };
            }
            
            presenter.Present(output);
        }
        
        Data.Soldier[] GenerateDefaultSquad() {
            var spawners = gameState.map.spawners;
            return new Data.Soldier[4] {
                SoldierGenerator.Default().At(spawners[0]).Build(),
                SoldierGenerator.Default().At(spawners[1]).Build(),
                SoldierGenerator.WithWeapon("Grenade Launcher").At(spawners[2]).Build(),
                SoldierGenerator.WithWeapon("Plasma Rifle").At(spawners[3]).Build()
            };
        }
    }
}
