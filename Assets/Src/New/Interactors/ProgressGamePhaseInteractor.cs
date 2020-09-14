using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Data;
using Workers;

namespace Interactors {
    
    public class ProgressGamePhaseInteractor : Interactor<ProgressGamePhaseOutput> {

        private const int SHOOTING_PHASE_ITERATIONS = 3;
        private const int MIN_SPAWN_DISTANCE = 12;

        [Dependency] GameState gameState;
        public IAlienStore alienStore { private get; set; }
        public ISoldierStore soldierStore { private get; set; }
        public IMissionStore missionStore { private get; set; }
        
        int stage;
        AlienSpawnerGenerator alienSpawnerGenerator;
        List<AlienSpawner> alienSpawners;

        public ProgressGamePhaseInteractor() {
            alienSpawners = new List<AlienSpawner>();
        }

        public void Interact(ProgressGamePhaseInput input) {
            var mission = missionStore.GetMission(gameState.campaign, gameState.mission);
            if (alienSpawnerGenerator == null) alienSpawnerGenerator = new AlienSpawnerGenerator(mission);
            var currentPhase = gameState.currentPhase;
            var result = new ProgressGamePhaseOutput();
            if (currentPhase == Data.GamePhase.Movement) {
                StartShootingPhase(ref result);
            } else if (stage >= SHOOTING_PHASE_ITERATIONS) {
                StartMovementPhase(ref result);
            } else {
                ProgressShootingPhase(ref result);
            }
            result.currentThreatLevel = gameState.currentThreatLevel;
            result.threatCountdown = mission.threatTimer - gameState.threatTimer;
            result.currentPart = stage;
            presenter.Present(result);
        }

        void StartShootingPhase(ref ProgressGamePhaseOutput result) {
            result.currentPhase = Data.GamePhase.Shooting;
            result.shootingStats = gameState.GetActors()
                                            .Where(actor => actor.isSoldier)
                                            .Select((actor) => {
                                                var soldier = actor as SoldierActor;
                                                var weaponStats = soldierStore.GetWeaponStats(soldier.weaponName);
                                                var armourStats = soldierStore.GetArmourStats(soldier.armourName);
                                                int shots;
                                                if (soldier.moved > armourStats.movement) {
                                                    shots = 0;
                                                } else if (soldier.moved > 0) {
                                                    shots = weaponStats.shotsWhenMoving;
                                                } else {
                                                    shots = weaponStats.shotsWhenStill;
                                                }
                                                return new ShootingStats { soldierID = soldier.uniqueId, shots = shots };
                                            }).ToArray();
            
            AlienPathingGrid.Calculate(gameState);
            
            stage = 0;
            gameState.SetCurrentPhase(Data.GamePhase.Shooting);
            
            foreach (var alien in Aliens.Iterate(gameState)) {
                alien.movesRemaining = alien.movement * SHOOTING_PHASE_ITERATIONS;
            }

            if (NoActionsToTake()) {
                ProgressShootingPhase(ref result);
            }
        }

        void ProgressShootingPhase(ref ProgressGamePhaseOutput result) {
            stage++;
            result.currentPhase = Data.GamePhase.Shooting;

            MoveAliens(ref result);

            while (stage < SHOOTING_PHASE_ITERATIONS && NoActionsToTake()) {
                stage++;
                MoveAliens(ref result);
            }
        }

        void StartMovementPhase(ref ProgressGamePhaseOutput result) {
            result.currentPhase = Data.GamePhase.Movement;
            var mission = missionStore.GetMission(gameState.campaign, gameState.mission);
            gameState.IncrementThreatTimer();
            if (gameState.threatTimer >= mission.threatTimer) {
                gameState.IncrementThreatLevel();
                alienSpawnerGenerator.EscalateThreat();
            }
            
            RemoveDeadSoldiers();

            gameState.SetCurrentPhase(Data.GamePhase.Movement);

            SpawnAliens(ref result);

            foreach (var actor in gameState.GetActors()) {
                if (actor is SoldierActor) {
                    var soldier = actor as SoldierActor;
                    soldier.shotsFiredThisTurn = 0;
                    soldier.moved = 0;
                } else if (actor is AlienActor) {
                    var alien = actor as AlienActor;
                    alien.movesRemaining = alien.movement * SHOOTING_PHASE_ITERATIONS;
                }
            }

            IncreaseShipEnergy(ref result);            
        }

        void IncreaseShipEnergy(ref ProgressGamePhaseOutput result) {
            if (gameState.shipEnergy.Increase()) result.shipEnergyEvent = new ShipEnergyEvent { netChange = 1 };
        }

        bool NoActionsToTake() {
            foreach (var actor in gameState.GetActors().Where(actor => actor is SoldierActor)) {
                var soldier = actor as SoldierActor;
                var weaponStats = soldierStore.GetWeaponStats(soldier.weaponName);
                var armourStats = soldierStore.GetArmourStats(soldier.armourName);
                var wrapper = new SoldierDecorator(soldier, weaponStats, armourStats);
                if (SoldierActions.ShootingActionsFor(gameState, wrapper).Any()) return false;
            }
            return true;
        }
        
        void SpawnAliens(ref ProgressGamePhaseOutput result) {
            var newAliens = new List<Data.Alien>();
            var radarBlips = new List<Position>();
            var newSpawners = alienSpawnerGenerator.Iterate();
            var spawnPoints = GetRandomSpawnPoints(newSpawners.Length);
            for (int i = 0; i < newSpawners.Length; i++) {
                newSpawners[i].position = spawnPoints[i];
            }
            alienSpawners.AddRange(newSpawners);
            
            var alienSpawns = new List<AlienSpawn>();
            foreach (var spawner in new List<AlienSpawner>(alienSpawners)) {
                var groupSize = UnityEngine.Random.Range(spawner.groupSizeMin, spawner.groupSizeMax);
                var types = new string[groupSize];
                for (int i = 0; i < groupSize; i++) {
                    types[i] = spawner.alienType;
                }
                alienSpawns.Add(new AlienSpawn(spawner.position, types));
                spawner.remainingIterations -= 1;
                if (spawner.remainingIterations <= 0) alienSpawners.Remove(spawner);
            }

            foreach (var alienSpawn in alienSpawns) {
                var aliens = alienSpawn.Execute(gameState, AlienPathingGrid.instance);
                foreach (var alien in aliens) {
                    var alienStats = alienStore.GetAlienStats(alien.alienType);
                    newAliens.Add(AddAlienToGameState(alien));
                    if (Random.Range(0, 100) <= alienStats.radarBlipChance) {
                        radarBlips.Add(alien.position);
                    }
                }
            }

            result.newAliens = newAliens.ToArray();
            result.radarBlips = radarBlips.ToArray();
        }

        Data.Alien AddAlienToGameState(Data.Alien alien) {
            alien.index = gameState.AddActor(AlienGenerator.FromStats(alienStore.GetAlienStats(alien.alienType)).At(alien.position).Build());
            return alien;
        } 

        void MoveAliens(ref ProgressGamePhaseOutput result) {
            var resultList = new List<AlienAction>();
            var pathingGrid = AlienPathingGrid.instance;
            var map = gameState.map;
            var aliensStillToMove = Aliens.Iterate(gameState).ToList();
            aliensStillToMove.ForEach(alien => alien.movesRemaining -= alien.movement);
            while (aliensStillToMove.Any()) {
                var aliensCopy = new List<AlienActor>(aliensStillToMove);
                foreach (var alien in aliensCopy) {
                    var iterator = new CellIterator(alien.position, cell => !cell.isWall && !cell.actor.isSoldier && (alien.position - cell.position).distance <= alien.movement);
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
                                aliensStillToMove.Contains(currentCell.actor)
                            )
                        ) {
                            bestSquare = currentSquare;
                        }
                    }
                    if (bestSquare == null) {
                        aliensStillToMove.Remove(alien);
                    } else if (!map.GetCell(bestSquare.position).hasActor) {
                        MoveAlien(alien, bestSquare.position);
                        aliensStillToMove.Remove(alien);
                        resultList.Add(new AlienAction {
                            index = alien.uniqueId,
                            type = AlienActionType.Move,
                            position = bestSquare.position,
                            facing = bestSquare.facing
                        });
                        resultList.AddRange(PerformAttackActions(alien));
                    } else if (map.GetCell(bestSquare.position).actor == alien) {
                        aliensStillToMove.Remove(alien);
                        resultList.AddRange(PerformAttackActions(alien));
                    }
                }
                if (aliensCopy.Count == aliensStillToMove.Count) break;
            }
            if (result.alienActions == null) {
                result.alienActions = resultList.ToArray();
            } else {
                result.alienActions = result.alienActions.Concat(resultList).ToArray();
            }
        }
        
        List<AlienAction> PerformAttackActions(AlienActor alien) {
            var result = new List<AlienAction>();
            foreach (var cell in new AdjacentCells(gameState.map).Iterate(alien.position)) {
                if (cell.actor is SoldierActor) {
                    if (!cell.actor.health.dead) {
                        result.Add(PerformAttack(alien, cell.actor as SoldierActor));
                    }
                }
            }
            return result;
        }
        
        void MoveAlien(AlienActor alien, Position position) {
            gameState.map.GetCell(alien.position).ClearActor();
            alien.position = position;
            gameState.map.GetCell(position).actor = alien;
        }
        
        AlienAction PerformAttack(AlienActor alien, SoldierActor soldier) {
            var alienStats = alienStore.GetAlienStats(alien.type);
            var armourStats = soldierStore.GetArmourStats(soldier.armourName);

            int damage = 0;
            AttackResult attackResult = AttackResult.Deflected;
            if (Random.Range(0, 100) >= armourStats.armourValue - alienStats.armourPen) {
                damage = alienStats.damage;
                attackResult = AttackResult.Hit;
                soldier.health.Damage(damage);
                if (soldier.health.dead) attackResult = AttackResult.Killed;
            }
            
            return new AlienAction {
                index = alien.uniqueId,
                type = AlienActionType.Attack,
                position = soldier.position,

                damageInstance = new DamageInstance {
                    perpetratorIndex = alien.uniqueId,
                    victimIndex = soldier.uniqueId,
                    damageInflicted = damage,
                    attackResult = attackResult,
                    victimHealthAfterDamage = soldier.health.current
                }
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

        void RemoveDeadSoldiers() {
            foreach (var soldier in Soldiers.Iterate(gameState)) {
                if (soldier.health.dead) {
                    gameState.RemoveActor(soldier.uniqueId);
                    metaGameState.metaSoldiers.Remove(soldier.metaSoldierId);
                }
            }
        }
    }
}
