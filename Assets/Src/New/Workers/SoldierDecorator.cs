using Data;

namespace Workers
{
    public class SoldierDecorator {

        [Dependency] ISoldierStore soldierStore;
        [Dependency] GameState gameState;

        SoldierActor soldier;

        public SoldierDecorator(SoldierActor soldier) {
            this.soldier = soldier;
        }

        public WeaponStats weaponStats => soldierStore.GetWeaponStats(soldier.weaponName);
        public ArmourStats armourStats => soldierStore.GetArmourStats(soldier.armourName);
        public CellType cell => gameState.map.GetCell(soldier.position);
        public long uniqueId => soldier.uniqueId;
        public string weaponName => soldier.weaponName;
        public Position position => soldier.position;
        public Direction facing => soldier.facing;
        public int movement => armourStats.movement;
        public int sprint => armourStats.sprint;
        public int remainingMovement => armourStats.movement + armourStats.sprint - soldier.moved;
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
        public int ammoSpent => soldier.ammoSpent;
        public int ammoRemaining => weaponStats.ammo - soldier.ammoSpent;

        public bool CanShoot() {
            return (soldier.moved <= 0 && soldier.shotsFiredThisTurn < weaponStats.shotsWhenStill ||
                   soldier.moved <= armourStats.movement && soldier.shotsFiredThisTurn < weaponStats.shotsWhenMoving)
                   && shotsRemaining > 0
                   && ammoRemaining > 0
                   && !soldier.shootingDisabled;
        }

        public void IncrementShotsFired() {
            soldier.shotsFiredThisTurn += 1;
            soldier.ammoSpent += 1;
        }

        public void IncrementMoved(int amount) {
            soldier.moved += amount;
        }

        public void RefillAmmo() {
            soldier.ammoSpent = 0;
        }

        public void DisableShooting() {
            soldier.shootingDisabled = true;
        }
    }
}