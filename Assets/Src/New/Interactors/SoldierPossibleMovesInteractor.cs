using Data;
using Workers;
using System.Collections.Generic;
using System.Linq;

namespace Interactors {
    
    public class SoldierPossibleMovesInteractor : Interactor<SoldierPossibleMovesOutput> {
        
        public void Interact(SoldierPossibleMovesInput input) {
            var soldier = gameState.GetActor(input.soldierIndex) as SoldierActor;
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
                                result.Add(checkedCell);
                            }
                        }
                    }
                }
                LeafCells = newLeafCells;
            }
            
            presenter.Present(new SoldierPossibleMovesOutput {
                possibleMoveLocations = result.Where(checkedCell =>
                    !checkedCell.cell.actor.exists &&
                    !checkedCell.cell.isFoggy
                ).Select(checkedCell => new PossibleMoveLocation {
                    position = checkedCell.cell.position,
                    sprint = soldier.moved + checkedCell.distance > soldier.baseMovement
                }).ToArray()
            });
        }
    }
    
    struct CheckedCell {
        
        public CellType cell;
        public int distance;
    }
}
