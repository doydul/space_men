using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Workers;

namespace Interactors {
    
    public class ActorActionsInteractor : Interactor<ActorActionsOutput> {

        public ISoldierStore soldierStore { private get; set; }

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
            var armour = soldierStore.GetArmourStats(soldier.armourName);
            var remainingMovement = armour.movement + armour.sprint - soldier.moved;
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
                    if (cell.distance >= remainingMovement) continue;
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
                    sprint = soldier.moved + checkedCell.distance > armour.movement
                }).ToList(); 
            actionsList.Add(new ActorAction {
                type = ActorActionType.Turn,
                index = soldier.uniqueId
            });
            output.actions = actionsList.ToArray();
        }

        void GetShootActions(long index, ref ActorActionsOutput output) {
            var actor = gameState.GetActor(index);
            if (actor is AlienActor) {
                GetAlienActions(index, ref output);
                return;
            }
            var soldier = actor as SoldierActor;
            var weaponStats = soldierStore.GetWeaponStats(soldier.weaponName);
            var armourStats = soldierStore.GetArmourStats(soldier.armourName);
            var wrapper = new SoldierDecorator(soldier, weaponStats, armourStats);
            var result = SoldierActions.ShootingActionsFor(gameState, wrapper);

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

        struct CheckedCell {
            public CellType cell;
            public int distance;
        }
    }
}
