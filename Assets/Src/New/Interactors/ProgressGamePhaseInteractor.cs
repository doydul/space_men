using Data;
using Workers;
using System.Collections.Generic;
using System.Linq;

namespace Interactors {
    
    public class ProgressGamePhaseInteractor : Interactor<ProgressGamePhaseOutput> {

        private const int SHOOTING_PHASE_ITERATIONS = 3;
        
        int stage;
        AlienSpawnerGenerator alienSpawnerGenerator;
        List<AlienSpawner> alienSpawners;
        
        public ProgressGamePhaseInteractor() {
            alienSpawnerGenerator = new AlienSpawnerGenerator(Storage.instance.GetCurrentMission());
            alienSpawners = new List<AlienSpawner>();
        }

        public void Interact(ProgressGamePhaseInput input) {
            var currentPhase = Storage.instance.GetCurrentPhase();
            if (currentPhase == Data.GamePhase.Movement) {
                StartShootingPhase();
            } else {
                ProgessShootingPhase();
            }
        }

        ProgressGamePhaseOutput StartShootingPhase() {
            var result = new ProgressGamePhaseOutput { currentPhase = Data.GamePhase.Shooting };
            
            stage = 0;
            Storage.instance.SetCurrentPhase(Data.GamePhase.Shooting);
            AlienTurnHack.SpawnAliens();
            
            SpawnAliens(ref result);
            
            return result;
        }

        ProgressGamePhaseOutput ProgessShootingPhase() {
            var result = new ProgressGamePhaseOutput();
            if (stage >= SHOOTING_PHASE_ITERATIONS) {
                StartMovementPhase();
                result.currentPhase = Data.GamePhase.Movement;
                return result;
            }
            stage++;
            result.currentPhase = Data.GamePhase.Shooting;
            AlienTurnHack.MoveAliens();
            // if player has no actions to take, proceed again
            return result;
        }

        void StartMovementPhase() {
            Storage.instance.SetCurrentPhase(Data.GamePhase.Movement);
            StartMovementPhaseHack.StartMovementPhase();
            // Reset soldier variables and change from sprint symbols to ammo symbols
        }
        
        void SpawnAliens(ref ProgressGamePhaseOutput result) {
            var newSpawners = alienSpawnerGenerator.Iterate();
            var spawnPoints = GetAvailableSpawnPoints(newSpawners.Length);
            for (int i = 0; i < newSpawners.Length; i++) {
                newSpawners[i].position = spawnPoints[i];
            }
            alienSpawners.AddRange(newSpawners);
            
            foreach (var spawner in alienSpawners) {
                // Add aliens to result
            }
        }
        
        Position[] GetAvailableSpawnPoints(int count) {
            // gameState.map.alienSpawners;
            // Filter out spawners that are too close or already occupied and return a random sample
            return null;
        }
    }
}
