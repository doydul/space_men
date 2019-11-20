using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Workers;

namespace Interactors {
    
    public class ActorActionsInteractor : Interactor<ActorActionsOutput> {

        public void Interact(ActorActionsInput input) {
            var output = new ActorActionsOutput();
            
            if (gameState.currentPhase == Data.GamePhase.Movement) {
                GetMoveActions(input.index, ref output);
            } else {
                GetShootActions(input.index, ref output);
            }
            
            presenter.Present(output);
        }

        void GetMoveActions(long index, ref ActorActionsOutput output) {
            var actor = gameState.GetActor(index);
            if (actor is AlienActor) {
                GetAlienActions(index, ref output);
                return;
            }
            var soldier = actor as SoldierActor;
            var map = gameState.map;
            
            var result = new List<CheckedCell>();
            var checkedPositions = new List<Position>() { soldier.position };
            var LeafCells = new List<CheckedCell>() { new CheckedCell {
                cell = map.GetCell(soldier.position.x, soldier.position.y),
                distance = 0
            } };

            while (LeafCells.Count > 0) {
                var newLeafCells = new List<CheckedCell>();
                foreach (var cell in LeafCells) {
                    if (cell.distance >= soldier.remainingMovement) continue;
                    foreach (var adjCell in new AdjacentCells(map).Iterate(cell.cell.position)) {
                        if (!checkedPositions.Contains(adjCell.position)) { 
                            if (!adjCell.isWall &&
                                (!adjCell.actor.exists || adjCell.actor is SoldierActor) &&
                                !(new AdjacentCells(map).Iterate(adjCell.position).Any(c => c.actor is AlienActor))) {
                                var checkedCell = new CheckedCell {
                                    cell = adjCell,
                                    distance = cell.distance + 1
                                };
                                newLeafCells.Add(checkedCell);
                                checkedPositions.Add(adjCell.position);
                                if (!adjCell.actor.exists && !adjCell.isFoggy) result.Add(checkedCell);
                            }
                        }
                    }
                }
                LeafCells = newLeafCells;
            }
            var actionsList = result.Select(checkedCell => new ActorAction {
                    type = ActorActionType.Move,
                    index = soldier.uniqueId,
                    target = checkedCell.cell.position,
                    sprint = soldier.moved + checkedCell.distance > soldier.baseMovement
                }).ToList(); 
            actionsList.Add(new ActorAction {
                type = ActorActionType.Turn,
                index = soldier.uniqueId
            });
            output.actions = actionsList.ToArray();
        }

        void GetShootActions(long index, ref ActorActionsOutput output) {
            var result = new List<ActorAction>();
            var actor = gameState.GetActor(index);
            if (actor is AlienActor) {
                GetAlienActions(index, ref output);
                return;
            }
            var soldier = actor as SoldierActor;

            foreach (var alien in Aliens.Iterate(gameState)) {
                if (
                    WithinSightArc(soldier.position, soldier.facing, alien.position) &&
                    !LineOfSightBlocked(soldier.position, alien.position)
                ) {
                    result.Add(new ActorAction {
                        index = soldier.uniqueId,
                        type = ActorActionType.Shoot,
                        target = alien.position,
                        actorTargetIndex = alien.uniqueId
                    });
                }
            }

            output.actions = result.ToArray();
        }

        void GetAlienActions(long index, ref ActorActionsOutput output) {
            var result = new List<Position>();
            var alien = gameState.GetActor(index) as AlienActor;
            var iterator = new CellIterator(alien.position, cell => !cell.isWall && !cell.actor.isSoldier);
            foreach (var node in iterator.Iterate(gameState.map)) {
                if (node.distanceFromStart > alien.movesRemaining) {
                    break;
                } else {
                    result.Add(node.cell.position);
                }
            }
            output.actions = result.Select((position) => {
                return new ActorAction {
                    index = alien.uniqueId,
                    type = ActorActionType.PossibleMove,
                    target = position
                };
            }).ToArray();
        }

        bool WithinSightArc(Position shooterPosition, Direction shooterFacing, Position targetPosition) {
            var distance = shooterPosition - targetPosition;
            if (shooterFacing == Direction.Up) {
                return distance.y < 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
            } else if (shooterFacing == Direction.Down) {
                return distance.y > 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
            } else if (shooterFacing == Direction.Left) {
                return distance.x > 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
            } else {
                return distance.x < 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
            }
        }

        bool LineOfSightBlocked(Position shooterPosition, Position targetPosition) {
            float blockage = 0f;
            var delta = targetPosition - shooterPosition;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                var ratio = delta.y / Mathf.Abs(delta.x);
                for (int i = 0; i < Mathf.Abs(delta.x) - 0.1f; i++) {
                    var location = new Position(shooterPosition.x + i * (int)Mathf.Sign(delta.x), Mathf.RoundToInt(shooterPosition.y + ratio * i));
                    blockage += Blockage(location);
                }
            } else {
                var ratio = delta.x / Mathf.Abs(delta.y);
                for (int i = 0; i < Mathf.Abs(delta.y) - 0.1f; i++) {
                    var location = new Position(Mathf.RoundToInt(shooterPosition.x + ratio * i), shooterPosition.y + i * (int)Mathf.Sign(delta.y));
                    blockage += Blockage(location);
                }
            }
            return blockage >= 1;
        }

        float Blockage(Position location) {
            var cell = gameState.map.GetCell(location);
            if (cell.isWall) {
                return 1;
            } else if (cell.actor.isSoldier) {
                return 0.5f;
            } else if (cell.actor.isAlien) {
                return 0.35f;
            }
            return 0;
        }

        struct CheckedCell {
            public CellType cell;
            public int distance;
        }
    }
}
