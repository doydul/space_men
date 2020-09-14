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

        public long uniqueId => soldier.uniqueId;
        public string weaponName => soldier.weaponName;
        public Position position => soldier.position;
        public Direction facing => soldier.facing;
        public int accuracy => weaponStats.accuracy;
        public int armourPen => weaponStats.armourPen;
        public int minDamage => weaponStats.minDamage;
        public int maxDamage => weaponStats.maxDamage;
        public int blast => (int)weaponStats.blast;
        public int totalShots { get {
            if (soldier.moved > armourStats.movement) {
                return 0;
            } else if (soldier.moved > 0) {
                return weaponStats.shotsWhenMoving;
            } else {
                return weaponStats.shotsWhenStill;
            }
        } }
        public int shotsRemaining => totalShots - soldier.shotsFiredThisTurn;
        public int maxAmmo => weaponStats.ammo;
        public int ammoRemaining => weaponStats.ammo - soldier.ammoSpent;

        public bool CanShoot() {
            return (soldier.moved <= 0 && soldier.shotsFiredThisTurn < weaponStats.shotsWhenStill ||
                   soldier.moved <= armourStats.movement && soldier.shotsFiredThisTurn < weaponStats.shotsWhenMoving)
                   && shotsRemaining > 0
                   && ammoRemaining > 0;
        }

        public void IncrementShotsFired() {
            soldier.shotsFiredThisTurn += 1;
            soldier.ammoSpent += 1;
        }

        public void IncrementMoved(int amount) {
            soldier.moved += amount;
        }
    }
}