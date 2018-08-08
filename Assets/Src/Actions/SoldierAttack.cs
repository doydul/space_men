using UnityEngine;

public static class SoldierAttack {
    
    public static void Execute(Soldier soldier, Alien alien, Map map) {
        soldier.ExpendAmmo();
        
        if (Random.value * 100 < soldier.accuracy + alien.accModifier) {
            
            if (Random.value * 100 > alien.armour - soldier.armourPen) {
                alien.ShowHitIndicator();
                alien.Hurt(soldier.damage);
                map.bloodSplatController.MakeSplat(alien);
            } else {
                alien.ShowDeflectIndicator();
            }
        }
    }
}