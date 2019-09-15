using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Data;
using Workers;

namespace Interactors {
    
    public class ProgressGamePhaseInteractor : Interactor<ProgressGamePhaseOutput> {

        private const int SHOOTING_PHASE_ITERATIONS = 3;
        private const int MIN_SPAWN_DISTANCE = 12;
        
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
                presenter.Present(StartShootingPhase());
            } else {
                presenter.Present(ProgessShootingPhase());
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
            foreach (var actor in gameState.GetActors()) {
                if (actor is SoldierActor) {
                    var soldier = actor as SoldierActor;
                    soldier.ammoSpent = 0;
                    soldier.moved = 0;
                }
            }
        }
        
        void SpawnAliens(ref ProgressGamePhaseOutput result) {
            var newAliens = new List<Data.Alien>();
            var newSpawners = alienSpawnerGenerator.Iterate();
            var spawnPoints = GetRandomSpawnPoints(newSpawners.Length);
            for (int i = 0; i < newSpawners.Length; i++) {
                newSpawners[i].position = spawnPoints[i];
            }
            alienSpawners.AddRange(newSpawners);
            
            foreach (var spawner in new List<AlienSpawner>(alienSpawners)) {
                if (spawner.spawnType == Data.AlienSpawnType.Trickle) {
                    newAliens.Add(new Data.Alien {
                       alienType = spawner.alienType,
                       position = spawner.position // TODO need to work out the position properly
                    });
                    spawner.remainingAliens -= 1;
                } else {
                    for (int i = 0; i < spawner.remainingAliens; i++) {
                        newAliens.Add(new Data.Alien {
                           alienType = spawner.alienType,
                           position = spawner.position // TODO need to work out the position properly
                        });
                    }
                    spawner.remainingAliens = 0;
                }
                if (spawner.remainingAliens <= 0) alienSpawners.Remove(spawner);
            }
            result.newAliens = newAliens.ToArray();
        }
        
        Position[] GetRandomSpawnPoints(int count) {
            var result = new Position[count];
            var filteredSpawners = gameState.map.alienSpawners.Where(gameStateSpawner => {
                return !alienSpawners.Any(spawner => spawner.position == gameStateSpawner);
            }).Where(gameStateSpawner => {
                return !gameState.GetActors().Any(actor => {
                    return actor is SoldierActor && (actor.position - gameStateSpawner).distance < MIN_SPAWN_DISTANCE;
                });
            }).ToList();
            
            for (int i = 0; i < count; i++) {
                var randomIndex = Random.Range(0, filteredSpawners.Count);
                result[i] = filteredSpawners[randomIndex];
                filteredSpawners.RemoveAt(randomIndex);
            }
            return result;
        }
    }
}
