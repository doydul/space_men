using UnityEngine;

public static class AlienAttack {
    
    public static void Execute(Alien alien, Soldier soldier, Map map) {
        if (Random.value * 100 > soldier.armour.armourValue - alien.armourPen) {
            soldier.ShowHitIndicator();
            int damage = alien.damage;
            if (Random.value < 0.2f) damage = damage * 4;
            soldier.Hurt(damage);
            map.bloodSplatController.MakeSplat(soldier);
        } else {
            soldier.ShowDeflectIndicator();
        }
    }
}