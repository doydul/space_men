using Data;
using Workers;

namespace Interactors {
    
    public class MissionStartInteractor : Interactor<MissionStartOutput> {

        public void Interact(MissionStartInput input) {
            var output = new MissionStartOutput();
            
            var squad = GenerateDefaultSquad(); // TODO: load from metagamestate instead
            output.soldiers = new Data.Soldier[squad.Length];
            
            int index = 0;
            foreach (var soldier in squad) {
                gameState.AddActor(soldier);
                output.soldiers[index] = soldier.ToValueType();
                index++;
            }
            
            AlienPathingGrid.Calculate(gameState);
            
            presenter.Present(output);
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
