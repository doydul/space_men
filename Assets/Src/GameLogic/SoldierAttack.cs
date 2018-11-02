using UnityEngine;

public static class SoldierAttack {

    public static void Execute(Soldier soldier, Alien alien, IWorld world) {
        soldier.ExpendAmmo();

        if (Random.value * 100 < soldier.accuracy + alien.accModifier) {

            if (Random.value * 100 > alien.armour - soldier.armourPen) {
                int damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                alien.ShowHitIndicator();
                alien.Hurt(damage);
                world.MakeBloodSplat(alien);
            } else {
                alien.ShowDeflectIndicator();
            }
        }
    }
}
