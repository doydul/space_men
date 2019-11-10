using System.Linq;
using UnityEngine;

using Data;
using Workers;
using System.Collections.Generic;

namespace Interactors {
    
    public class SoldierShootInteractor : Interactor<SoldierShootOutput> {

        public IAlienStore alienStore { private get; set; }
        public ISoldierStore soldierStore { private get; set; }

        public void Interact(SoldierShootInput input) {
            var output = new SoldierShootOutput();
            
            var soldier = gameState.GetActor(input.index) as SoldierActor;
            var alien = gameState.GetActor(input.targetIndex) as AlienActor;
            var soldierDecorator = new SoldierDecorator(
                soldier,
                soldierStore.GetWeaponStats(soldier.weaponName),
                soldierStore.GetArmourStats(soldier.armourName)
            );
            var alienDecorator = new AlienDecorator(
                alien,
                alienStore.GetAlienStats(alien.type)
            );

            output.soldierIndex = input.index;
            output.weaponName = soldierDecorator.weaponName;

            if (!soldierDecorator.CanShoot()) return;

            soldierDecorator.IncrementAmmoSpent();
            output.ammoSpent = 1;

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
            if (Random.Range(0, 100) < soldier.accuracy) {
                if (Random.Range(0, 100) - soldier.armourPen < alien.armour) {
                    damageInstance.damageInflicted = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                    damageInstance.attackResult = AttackResult.Hit;
                    alien.Damage(damageInstance.damageInflicted);
                    damageInstance.victimHealthAfterDamage = alien.currentHealth;
                    if (alien.dead) {
                        damageInstance.attackResult = AttackResult.Killed;
                        gameState.RemoveActor(alien.uniqueId);
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
            var explosionTiles = GenerateExplosion(alien.position, soldier.accuracy, soldier.blast);
            var damageInstances = new List<DamageInstance>();
            foreach (var tilePosition in explosionTiles) {
                var cell = map.GetCell(tilePosition);
                int armour;
                Health health;
                if (cell.actor.isAlien) {
                    var cellAlien = cell.actor as AlienActor;
                    armour = alienStore.GetAlienStats(cellAlien.type).armour;
                    health = cellAlien.health;
                } else if (cell.actor.isSoldier) {
                    var cellSoldier = cell.actor as SoldierActor;
                    armour = soldierStore.GetArmourStats(cellSoldier.armourName).armourValue;
                    health = cellSoldier.health;
                } else {
                    continue;
                }
                if (cell.actor.exists) {
                    var damageInstance = new DamageInstance {
                        perpetratorIndex = soldier.uniqueId,
                        victimIndex = cell.actor.uniqueId
                    };
                    if (Random.Range(0, 100) >= armour) {
                        var damage = Random.Range(soldier.minDamage, soldier.maxDamage + 1);
                        health.Damage(damage);
                        damageInstance.damageInflicted = damage;
                        damageInstance.attackResult = AttackResult.Hit;
                        damageInstance.victimHealthAfterDamage = health.current;
                        if (health.dead) {
                            damageInstance.attackResult = AttackResult.Killed;
                            gameState.RemoveActor(cell.actor.uniqueId);
                        }
                    } else {
                        damageInstance.attackResult = AttackResult.Deflected;
                    }
                    damageInstances.Add(damageInstance);
                }
            }
            output.damageInstances = damageInstances.ToArray();
            output.blastCoverage = explosionTiles.ToArray();
        }

        Position[] GenerateExplosion(Position targetPosition, int accuracy, int blastSize) {
            var result = new List<Position>();
            var realPosition = targetPosition;
            if (Random.Range(0, 100) >= accuracy) {
                realPosition = ScatterOrdnance(targetPosition);
            }

            var iterator = new CellLayerIterator(targetPosition, cell => !cell.isWall);
            int layerI = -4;
            foreach (var layer in iterator.Iterate(gameState.map)) {
                blastSize -= Mathf.Max(layerI, 0);
                foreach (var node in layer.nodes.OrderBy(x => Random.value).Take(blastSize)) {
                    result.Add(node.cell.position);
                    blastSize--;
                }
                if (blastSize <= 0) break;
                layerI += 2;
            }
            return result.ToArray();
        }

        Position ScatterOrdnance(Position targetPosition) {
            int scatterDistance = Random.value < 0.5f ? 1 : 2;
            var iterator = new CellLayerIterator(targetPosition, cell => !cell.isWall);
            foreach (var layer in iterator.Iterate(gameState.map)) {
                if (layer.distanceFromStart == scatterDistance) {
                    return layer.nodes[Random.Range(0, layer.nodes.Length)].cell.position;
                } else if (layer.distanceFromStart > scatterDistance) {
                    break;
                }
            }
            return targetPosition;
        }
    }
}
