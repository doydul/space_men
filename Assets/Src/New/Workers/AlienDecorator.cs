using Data;

namespace Workers
{
    public class AlienDecorator
    {
        AlienActor alien;
        AlienStats stats;

        public AlienDecorator(AlienActor alien, AlienStats stats) {
            this.alien = alien;
            this.stats = stats;
        }

        public long uniqueId { get { return alien.uniqueId; } }
        public Position position { get { return alien.position; } }
        public int armour { get { return stats.armour; } }
        public bool dead { get { return alien.health.dead; } }

        public void Damage(int amount) {
            alien.health.Damage(amount);
        }
    }
}