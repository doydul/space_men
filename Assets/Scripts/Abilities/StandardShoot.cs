using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "StandardShoot", menuName = "Abilities/Standard Shoot", order = 1)]
public class StandardShoot : Ability {
    
    public override bool CanUse() {
        return owner.canShoot;
    }
}