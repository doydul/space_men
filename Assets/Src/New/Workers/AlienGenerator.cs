using Data;

namespace Workers {
    
    public static class AlienGenerator {
        
        public static Builder FromStats(AlienStats stats) {
            return new Builder(new AlienActor {
                health = new Health(stats.maxHealth),
                movement = stats.movement,
                type = stats.name
            });
        }
        
        public class Builder {
            
            AlienActor actor;
            
            public Builder(AlienActor actor) {
                this.actor = actor;
            }
            
            public AlienActor Build() {
                return actor;
            }

            public Builder At(Position position) {
                actor.position = position;
                return this;
            }
        }
    }
}
