using Data;
using System;

namespace Workers {

    public class SoldierMove {

        int soldierIndex;
        Position targetLocation;

        public SoldierMove(int soldierIndex, Position targetLocation) {
            this.soldierIndex = soldierIndex;
            this.targetLocation = targetLocation;
        }

        public void Execute(GameState gameState) {
            var map = gameState.map;
            var targetCell = map.GetCell(targetLocation);
            if (targetCell.actorType != ActorType.None) throw new Exception("Soldier Move: Target location is already occupied");
            var soldier = gameState.GetSoldier(soldierIndex);
            var currentCell = map.GetCell(soldier.position);
            
            soldier.position = targetLocation;
            
            targetCell.actorType = ActorType.Soldier;
            targetCell.soldierIndex = soldierIndex;

            currentCell.actorType = ActorType.None;

            gameState.UpdateSoldier(soldierIndex, soldier);
            map.UpdateCell(targetCell.position, targetCell);
            map.UpdateCell(currentCell.position, currentCell);
        }
    }    
}
