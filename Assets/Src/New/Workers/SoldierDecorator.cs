using Data;

namespace Workers
{
    public class SoldierDecorator
    {
        SoldierActor soldier;
        WeaponStats weaponStats;
        ArmourStats armourStats;

        public SoldierDecorator(SoldierActor soldier, WeaponStats weaponStats, ArmourStats armourStats) {
            this.soldier = soldier;
            this.weaponStats = weaponStats;
            this.armourStats = armourStats;
        }

        public long uniqueId { get { return soldier.uniqueId; } }
        public string weaponName { get { return soldier.weaponName; } }
        public Position position { get { return soldier.position; } }
        public Direction facing { get { return soldier.facing; } }
        public int accuracy { get { return weaponStats.accuracy; } }
        public int armourPen { get { return weaponStats.armourPen; } }
        public int minDamage { get { return weaponStats.minDamage; } }
        public int maxDamage { get { return weaponStats.maxDamage; } }
        public int blast { get { return (int)weaponStats.blast; } }
        public int totalShots { get {
            if (soldier.moved > armourStats.movement) {
                return 0;
            } else if (soldier.moved > 0) {
                return weaponStats.shotsWhenMoving;
            } else {
                return weaponStats.shotsWhenStill;
            }
        } }
        public int shotsRemaining { get { return totalShots - soldier.ammoSpent; } }

        public bool CanShoot() {
            return soldier.moved <= 0 && soldier.ammoSpent < weaponStats.shotsWhenStill ||
                   soldier.moved <= armourStats.movement && soldier.ammoSpent < weaponStats.shotsWhenMoving;
        }

        public void IncrementAmmoSpent() {
            soldier.ammoSpent += 1;
        }

        public void IncrementMoved(int amount) {
            soldier.moved += amount;
        }
    }
}