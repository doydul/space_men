using System.Collections.Generic;
using System.Linq;
using Data;

namespace Workers {
    
    public static class FogHandler {

        const int FOG_DISTANCE = 7;
        
        public static Fog[] ApplyFog(GameState gamestate) {
            var result = new List<Fog>();
            var map = gamestate.map;
            foreach (var cell in map.cells) {
                cell.isFoggy = true;
            }
            var soldiers = gamestate.GetActors().Where(actor => actor.isSoldier);
            foreach (var soldier in soldiers) {
                var iterator = new CellIterator(soldier.position, cell => !cell.isWall);
                foreach (var node in iterator.Iterate(map)) {
                    if (node.distanceFromStart >= FOG_DISTANCE) break;
                    node.cell.isFoggy = false;
                }
            }
            foreach (var cell in map.cells) {
                if (cell.isFoggy) result.Add(new Fog { position = cell.position });
            }
            return result.ToArray();
        }
    }
}
