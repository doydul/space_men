using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class MissionStartInteractor : Interactor<MissionStartOutput> {

        public void Interact(MissionStartInput input) {
            var output = new MissionStartOutput();
            
            var squad = metaGameState.metaSoldiers.GetAll().Select(metaSoldier => SoldierFromMetaSoldier(metaSoldier)).ToList();
            for (int i = 0; i < squad.Count; i++) {
                squad[i].position = gameState.map.spawners[i];
            }
            output.soldiers = new Data.Soldier[squad.Count];
            
            int index = 0;
            foreach (var soldier in squad) {
                gameState.AddActor(soldier);
                output.soldiers[index] = soldier.ToValueType();
                index++;
            }
            AlienPathingGrid.Calculate(gameState);

            output.fogs = FogHandler.ApplyFog(gameState);
            
            presenter.Present(output);
        }

        SoldierActor SoldierFromMetaSoldier(MetaSoldier metaSoldier) {
            return SoldierGenerator.Default()
                                   .WithArmour(metaSoldier.armour.name)
                                   .WithWeapon(metaSoldier.weapon.name)
                                   .Build();
        }
        
        SoldierActor[] GenerateDefaultSquad() {
            var spawners = gameState.map.spawners;
            return new SoldierActor[4] {
                SoldierGenerator.Default().At(spawners[0]).Build(),
                SoldierGenerator.Default().At(spawners[1]).Build(),
                SoldierGenerator.WithWeapon("Grenade Launcher").At(spawners[2]).Build(),
                SoldierGenerator.WithWeapon("Plasma Rifle").At(spawners[3]).Build()
            };
        }
    }
}
