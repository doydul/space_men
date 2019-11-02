using System.Collections.Generic;

using Data;

namespace Workers {
    
    public static class Aliens {
        
        public static IEnumerable<AlienActor> Iterate(GameState gameState) {
            foreach (var actor in gameState.GetActors()) {
                if (actor is AlienActor) yield return actor as AlienActor;
            }
        }
    }
}
