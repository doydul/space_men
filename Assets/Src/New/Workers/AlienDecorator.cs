using Data;

namespace Workers {
    public class AlienDecorator {

        [Dependency] IAlienStore alienStore;

        AlienActor alien;

        public AlienDecorator(AlienActor alien) {
            this.alien = alien;
        }

        public AlienStats stats => alienStore.GetAlienStats(alien.type);
        public long uniqueId => alien.uniqueId;
        public Position position => alien.position;
        public int armour => stats.armour;
        public int accModifier => stats.accModifier;
        public bool dead => alien.health.dead;
        public int currentHealth => alien.health.current;
        public int expReward => stats.expReward;

        public void Damage(int amount) {
            alien.health.Damage(amount);
        }
    }
}