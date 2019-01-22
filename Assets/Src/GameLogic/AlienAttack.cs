using UnityEngine;

public static class AlienAttack {

    public static void Execute(Alien alien, Soldier soldier, IWorld world) {
        if (Random.value * 100 > soldier.armour.armourValue - alien.armourPen) {
            soldier.ShowHitIndicator();
            int damage = alien.damage;
            if (Random.value < 0.2f) damage = (int)(damage * 2.5f);
            soldier.Hurt(damage);
            world.MakeBloodSplat(soldier);
        } else {
            soldier.ShowDeflectIndicator();
        }
    }
}
