using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "SuicideBehaviour", menuName = "Behaviours/Suicide", order = 2)]
public class SuicideBehaviour : AlienBehaviour {

    public Weapon explosionWeapon;

    public override IEnumerator PerformTurn() {
        yield break;
    }
}
