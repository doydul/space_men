using Data;

namespace Workers {
    
    public static class AlienGenerator {
        
        public static Builder FromValueType(Data.Alien alienStruct) {
            return new Builder(new AlienActor {
                position = alienStruct.position,
                health = new Health(1),
                movement = 3
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
        }
    }
}
