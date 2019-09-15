using System.Collections.Generic;
using System;

using Data;

namespace Workers {
    
    public class AlienSpawn {
        
        private const int MIN_SPAWN_DISTANCE = 7;
        private const int MAX_SPAWN_DISTANCE = 13;
        
        Position spawnPoint;
        Data.Alien[] aliens;
        
        public AlienSpawn(Position spawnPoint, Data.Alien[] aliens) {
            this.spawnPoint = spawnPoint;
            this.aliens = aliens;
        }
        
        public void Execute(GameState gameState) {
            var iterator = new CellIterator(spawnPoint, cell => !cell.isWall);
            
            foreach (var node in iterator.Iterate(gameState.map)) {
                // YOU ARE HERE
            }
        }
    }
}
