using System.Linq;
using Data;
using Workers;

namespace Interactors {
    
    public class MissionStartInteractor : Interactor<MissionStartOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;

        public void Interact(MissionStartInput input) {
            var output = new MissionStartOutput();
            
            var squad = metaGameState.metaSoldiers.GetSquad()
                                                  .Where(soldier => soldier != null)
                                                  .Select(metaSoldier => SoldierFromMetaSoldier(metaSoldier))
                                                  .Take(gameState.map.spawners.Length)
                                                  .ToList();
            for (int i = 0; i < squad.Count; i++) {
                squad[i].position = gameState.map.spawners[i];
            }
            output.soldiers = new SoldierDisplayInfo[squad.Count];
            
            int index = 0;
            foreach (var soldier in squad) {
                gameState.AddActor(soldier);
                output.soldiers[index] = factory.MakeObject<SoldierDecorator>(soldier).GenerateDisplayInfo();
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
                                   .WithMetaSoldierId(0)
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
