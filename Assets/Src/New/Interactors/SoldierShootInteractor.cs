using System.Linq;
using UnityEngine;

using Data;
using Workers;
using System.Collections.Generic;

namespace Interactors {
    
    public class SoldierShootInteractor : Interactor<SoldierShootOutput> {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;
        [Dependency] IAlienStore alienStore;
        [Dependency] ISoldierStore soldierStore;

        public void Interact(SoldierShootInput input) {
            var output = new SoldierShootOutput();
            
            var soldier = gameState.GetActor(input.index) as SoldierActor;
            var alien = gameState.GetActor(input.targetIndex) as AlienActor;
            var soldierDecorator = factory.MakeObject<SoldierDecorator>(soldier);
            var alienDecorator = factory.MakeObject<AlienDecorator>(alien);

            output.soldierIndex = input.index;
            output.weaponName = soldierDecorator.weaponName;

            if (!soldierDecorator.CanShoot()) return;

            soldierDecorator.IncrementShotsFired();
            output.shotsLeft = soldierDecorator.shotsRemaining;
            output.ammoLeft = soldierDecorator.ammoRemaining;
            output.maxAmmo = soldierDecorator.maxAmmo;

            if (soldierDecorator.blast > 0) {
                ShootBlastWeapon(soldierDecorator, alienDecorator, ref output);
            } else {
                ShootNormalWeapon(soldierDecorator, alienDecorator, ref output);
            }
            
            presenter.Present(output);
        }

        void ShootNormalWeapon(SoldierDecorator soldier, AlienDecorator alien, ref SoldierShootOutput output) {
            var damageInstance = new DamageInstance {
                perpetratorIndex = soldier.uniqueId,
                victimIndex = alien.uniqueId
            };
            if (Random.Range(0, 100) < soldier.accuracy + alien.accModifier) {
                if (Random.Range(0, 100) > alien.armour - soldier.armourPen) {
                    damageInstance.damageInflicted = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                    damageInstance.attackResult = AttackResult.Hit;
                    alien.Damage(damageInstance.damageInflicted);
                    damageInstance.victimHealthAfterDamage = alien.currentHealth;
                    if (alien.dead) {
                        damageInstance.attackResult = AttackResult.Killed;
                        gameState.RemoveActor(alien.uniqueId);
                        soldier.GainExp(alien.expReward);
                    }
                } else {
                    damageInstance.attackResult = AttackResult.Deflected;
                }
            } else {
                damageInstance.attackResult = AttackResult.Missed;
            }
            output.damageInstances = new DamageInstance[] { damageInstance };
        }

        void ShootBlastWeapon(SoldierDecorator soldier, AlienDecorator alien, ref SoldierShootOutput output) {
            var map = gameState.map;

            var explosion = factory.MakeObject<Workers.Explosion>();
            explosion.CalculateFromSoldier(soldier.uniqueId, alien.position);
            output.explosion = new ExplosionData {
                squaresCovered = explosion.coveredTiles,
                damageInstances = explosion.damageInstances,
                fires = explosion.fires
            };
        }
    }
}
