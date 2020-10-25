using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Workers {

    public class Explosion {

        [Dependency] GameState gameState;
        [Dependency] IInstantiator factory;
        [Dependency] IAlienStore alienStore;
        [Dependency] ISoldierStore soldierStore;

        public Position[] coveredTiles { get; private set; }
        public DamageInstance[] damageInstances { get; private set; }

        public void CalculateFromSoldier(long soldierId, Position target) {
            var soldier = factory.MakeObject<SoldierDecorator>(gameState.GetActor(soldierId) as SoldierActor);
            var config = new Config {
                soldierId = soldierId,
                target = target,
                accuracy = soldier.accuracy,
                blastSize = soldier.blast,
                minDamage = soldier.minDamage,
                maxDamage = soldier.maxDamage
            };
            Calculate(config);
        }

        public void Calculate(Config config) {
            var explosionTiles = CalculateCoverage(config.target, config.accuracy, config.blastSize);
            var soldier = factory.MakeObject<SoldierDecorator>(gameState.GetActor(config.soldierId) as SoldierActor);
            var damageInstances = new List<DamageInstance>();
            foreach (var tilePosition in explosionTiles) {
                var cell = gameState.map.GetCell(tilePosition);
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
                        perpetratorIndex = config.soldierId,
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
                            if (cell.actor.isAlien) {
                                var cellAlien = cell.actor as AlienActor;
                                soldier.GainExp(alienStore.GetAlienStats(cellAlien.type).expReward);
                            }
                        }
                    } else {
                        damageInstance.attackResult = AttackResult.Deflected;
                    }
                    damageInstances.Add(damageInstance);
                }
            }
            this.damageInstances = damageInstances.ToArray();
            coveredTiles = explosionTiles.ToArray();
        }

        Position[] CalculateCoverage(Position targetPosition, int accuracy, int blastSize) {
            var result = new List<Position>();
            var realPosition = targetPosition;
            if (Random.Range(0, 100) >= accuracy) {
                realPosition = ScatterOrdnance(targetPosition);
            }

            var iterator = new CellLayerIterator(targetPosition, cell => !cell.isWall);
            int layerI = -4;
            foreach (var layer in iterator.Iterate(gameState.map)) {
                // Try without blast damping
                // blastSize -= Mathf.Max(layerI, 0); 
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

        public struct Config {
            public long soldierId;
            public Position target;
            public int accuracy;
            public int blastSize;
            public int minDamage;
            public int maxDamage;
        }
    }
}