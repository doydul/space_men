using System.Collections.Generic;

using Data;

namespace Workers {
    
    public static class Soldiers {
        
        public static IEnumerable<SoldierActor> Iterate(GameState gameState) {
            foreach (var actor in gameState.GetActors()) {
                if (actor is SoldierActor) yield return actor as SoldierActor;
            }
        }
    }
}
