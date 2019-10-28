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
            } else if (stage >= SHOOTING_PHASE_ITERATIONS) {
                presenter.Present(StartMovementPhase());
            } else {
                presenter.Present(ProgessShootingPhase());
            }
        }

        ProgressGamePhaseOutput StartShootingPhase() {
            var result = new ProgressGamePhaseOutput { currentPhase = Data.GamePhase.Shooting };
            
            AlienPathingGrid.Calculate(gameState);
            
            stage = 0;
            Storage.instance.SetCurrentPhase(Data.GamePhase.Shooting);
            
            SpawnAliens(ref result);
            
            return result;
        }

        ProgressGamePhaseOutput ProgessShootingPhase() {
            var result = new ProgressGamePhaseOutput();
            stage++;
            result.currentPhase = Data.GamePhase.Shooting;

            MoveAliens(ref result);

            // if player has no actions to take, proceed again
            return result;
        }

        ProgressGamePhaseOutput StartMovementPhase() {
            var result = new ProgressGamePhaseOutput();
            result.currentPhase = Data.GamePhase.Movement;

            Storage.instance.SetCurrentPhase(Data.GamePhase.Movement);
            StartMovementPhaseHack.StartMovementPhase();
            foreach (var actor in gameState.GetActors()) {
                if (actor is SoldierActor) {
                    var soldier = actor as SoldierActor;
                    soldier.ammoSpent = 0;
                    soldier.moved = 0;
                }
            }
            return result;
        }
        
        void SpawnAliens(ref ProgressGamePhaseOutput result) {
            var newAliens = new List<Data.Alien>();
            var newSpawners = alienSpawnerGenerator.Iterate();
            var spawnPoints = GetRandomSpawnPoints(newSpawners.Length);
            for (int i = 0; i < newSpawners.Length; i++) {
                newSpawners[i].position = spawnPoints[i];
            }
            alienSpawners.AddRange(newSpawners);
            
            var alienSpawns = new List<AlienSpawn>();
            foreach (var spawner in new List<AlienSpawner>(alienSpawners)) {
                if (spawner.spawnType == Data.AlienSpawnType.Trickle) {
                    alienSpawns.Add(new AlienSpawn(spawner.position, new AlienType[] { spawner.alienType }));
                    spawner.remainingAliens -= 1;
                } else {
                    var types = new AlienType[spawner.remainingAliens];
                    for (int i = 0; i < spawner.remainingAliens; i++) {
                        types[i] = spawner.alienType;
                    }
                    alienSpawns.Add(new AlienSpawn(spawner.position, types));
                    spawner.remainingAliens = 0;
                }
                if (spawner.remainingAliens <= 0) alienSpawners.Remove(spawner);
            }

            foreach (var alienSpawn in alienSpawns) {
                var aliens = alienSpawn.Execute(gameState, AlienPathingGrid.instance);
                foreach (var alien in aliens) {
                    newAliens.Add(AddAlienToGameState(alien));
                }
            }

            result.newAliens = newAliens.ToArray();
        }

        Data.Alien AddAlienToGameState(Data.Alien alien) {
            alien.index = gameState.AddActor(AlienGenerator.FromValueType(alien).Build());
            return alien;
        } 

        void MoveAliens(ref ProgressGamePhaseOutput result) {
            var resultList = new List<AlienAction>();
            var pathingGrid = AlienPathingGrid.instance;
            var map = gameState.map;
            var aliens = Aliens.Iterate(gameState).ToList();
            var aliensCopy = new List<AlienActor>(aliens);
            while (aliens.Any()) {
                foreach (var alien in aliensCopy) {
                    var iterator = new CellIterator(alien.position, cell => !cell.isWall && (alien.position - cell.position).distance <= alien.movement);
                    AlienPathingGrid.GridSquare bestSquare = null;
                    foreach (var node in iterator.Iterate(map)) {
                        var currentSquare = pathingGrid.GetSquare(node.cell.position);
                        var currentCell = map.GetCell(currentSquare.position);
                        if (
                            (
                                bestSquare == null ||
                                bestSquare.distanceToNearestSoldier > currentSquare.distanceToNearestSoldier
                            ) &&
                            (
                                !currentCell.hasActor ||
                                aliens.Contains(currentCell.actor)
                            )
                        ) {
                            bestSquare = currentSquare;
                        }
                    }
                    if (!map.GetCell(bestSquare.position).hasActor) {
                        MoveAlien(alien, bestSquare.position);
                        aliens.Remove(alien);
                        resultList.Add(new AlienAction {
                            index = alien.uniqueId,
                            type = AlienActionType.Move,
                            position = bestSquare.position,
                            facing = bestSquare.facing
                        });
                        resultList.AddRange(PerformAttackActions(alien));
                    } else if (map.GetCell(bestSquare.position).actor == alien) {
                        aliens.Remove(alien);
                    }
                }
                if (aliensCopy.Count == aliens.Count) break;
            }
            result.alienActions = resultList.ToArray();
        }
        
        List<AlienAction> PerformAttackActions(AlienActor alien) {
            // var result = new List<AlienAction>();
            // foreach (var cell in new AdjacentCells(gameState.map).Iterate(alien.position)) {
            //     if (cell.actor is SoldierActor) {
            //         result.Add(PerformAttack(alien, cell.actor as SoldierActor));
            //     }
            // }
            return null;
        }
        
        void MoveAlien(AlienActor alien, Position position) {
            gameState.map.GetCell(alien.position).ClearActor();
            alien.position = position;
            gameState.map.GetCell(position).actor = alien;
        }
        
        AlienAction PerformAttack(AlienActor alien, SoldierActor soldier) {
            // Calculate damage and attack result, using alien stats from some sort of alien fetcher worker
            
            return new AlienAction {
                index = alien.uniqueId,
                type = AlienActionType.Attack,
                position = soldier.position,
                // damage = damage,
                // attackResult = attackResult
            };
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
