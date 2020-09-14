using Data;
using Workers;

namespace Interactors {
    
    public class SpawnAliensInteractor : Interactor<SpawnAliensOutput> {

        [Dependency] GameState gameState;
        public IAlienStore alienStore { private get; set; }

        public void Interact(SpawnAliensInput input) {
            var output = new SpawnAliensOutput();
            
            // TODO allow adding multiple aliens
            var alien = new Data.Alien {
                alienType = input.alienType,
                position = new Position(input.xPos, input.yPos),
                facing = input.facing
            };

            output.newAliens = new Data.Alien[] {
                AddAlienToGameState(alien)
            };
            
            presenter.Present(output);
        }

        Data.Alien AddAlienToGameState(Data.Alien alien) {
            alien.index = gameState.AddActor(AlienGenerator.FromStats(alienStore.GetAlienStats(alien.alienType)).At(alien.position).Build());
            return alien;
        } 
    }
}
