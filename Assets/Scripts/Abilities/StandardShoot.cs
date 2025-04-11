using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "StandardShoot", menuName = "Abilities/Standard Shoot", order = 1)]
public class StandardShoot : Ability {
    
    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new HasAmmo();
        yield return new HasAction();
    }
}