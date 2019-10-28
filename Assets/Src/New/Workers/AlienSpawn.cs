using System.Collections.Generic;
using System;
using UnityEngine;

using Data;

namespace Workers {
    
    public class AlienSpawn {
        
        private const int MIN_SPAWN_DISTANCE = 7;
        private const int MAX_SPAWN_DISTANCE = 13;
        
        Position spawnPoint;
        Queue<Data.AlienType> aliens;

        public AlienSpawn(Position spawnPoint, Data.AlienType[] aliens) {
            this.spawnPoint = spawnPoint;
            this.aliens = new Queue<Data.AlienType>(aliens);
        }
        
        public Data.Alien[] Execute(GameState gameState, AlienPathingGrid pathingGrid) {
            var result = new List<Data.Alien>();
            int spawnDistance = UnityEngine.Random.Range(MIN_SPAWN_DISTANCE, MAX_SPAWN_DISTANCE);
            
            var squareIteration = pathingGrid.GetSquare(spawnPoint);
            if (squareIteration.distanceToNearestSoldier < MIN_SPAWN_DISTANCE) return result.ToArray();
            while (squareIteration.distanceToNearestSoldier > spawnDistance) {
                squareIteration = squareIteration.nextSquare;
            }

            var iterator = new CellIterator(squareIteration.position, cell => !cell.isWall);
            foreach (var node in iterator.Iterate(gameState.map)) {
                var square = pathingGrid.GetSquare(node.cell.position);
                if (!node.cell.hasActor && square.distanceToNearestSoldier >= MIN_SPAWN_DISTANCE) {
                    var alienType = aliens.Dequeue();
                    result.Add(new Data.Alien {
                        alienType = alienType,
                        position = node.cell.position,
                        facing = square.facing
                    });
                }
                if (aliens.Count <= 0) break;
            }
            return result.ToArray();
        }
    }
}
