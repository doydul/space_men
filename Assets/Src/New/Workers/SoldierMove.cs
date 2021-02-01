using System.Collections.Generic;
using System.Linq;

using Data;
using System;

namespace Workers {

    public class SoldierMove {

        long soldierIndex;
        Position targetPosition;

        public Position[] traversedCells { get; private set; }
        public DamageInstance[] damageInstances { get; private set; }

        public SoldierMove(long soldierIndex, Position targetPosition) {
            this.soldierIndex = soldierIndex;
            this.targetPosition = targetPosition;
        }

        public void Execute(GameState gameState, MetaGameState metaGameState) {
            var map = gameState.map;
            var targetCell = map.GetCell(targetPosition);
            if (targetCell.actor.exists) throw new Exception("Soldier Move: Target location is already occupied");
            var soldier = gameState.GetActor(soldierIndex) as SoldierActor;
            var currentCell = map.GetCell(soldier.position);
            
            var path = GetPath(map, currentCell.position, targetCell.position);
            
            traversedCells = path.Nodes().Select(node => node.position).Where(pos => pos != soldier.position).ToArray();
            var damageInstancesList = new List<DamageInstance>();
            foreach (var position in traversedCells) {
                var cell = map.GetCell(position);
                if (cell.backgroundActor.isFlame) {
                    var flame = cell.backgroundActor as FlameActor;
                    soldier.health.Damage(flame.damage);
                    if (soldier.health.dead) {
                        damageInstancesList.Add(new DamageInstance {
                            attackResult = AttackResult.Killed
                        });
                        gameState.RemoveActor(soldier.uniqueId);
                        metaGameState.metaSoldiers.Remove(soldier.metaSoldierId);
                        break;
                    }
                    damageInstancesList.Add(new DamageInstance {
                        damageInflicted = flame.damage,
                        attackResult = AttackResult.Hit,
                        perpetratorIndex = -1,
                        victimIndex = soldierIndex,
                        victimHealthAfterDamage = soldier.health.current
                    });
                }
            }
            damageInstances = damageInstancesList.ToArray();
            soldier.moved += path.length;
            soldier.facing = path.facing;
            soldier.position = targetPosition;
            targetCell.actor = soldier;
            currentCell.ClearActor();
        }
        
        Node GetPath(MapState map, Position start, Position finish) {
            var iterator = new AdjacentCells(map);

            var checkedPositions = new List<Position>() { start };
            var leafNodes = new List<Node>() { new Node(start, Direction.Up, 0, null) };
            while (leafNodes.Count > 0) {
                var newLeafNodes = new List<Node>();
                foreach (var node in leafNodes) {
                    foreach (var adjCell in iterator.Iterate(node.position)) {
                        if (!checkedPositions.Contains(adjCell.position) &&
                            !adjCell.isWall &&
                            (!adjCell.actor.exists || adjCell.actor is SoldierActor)) {
                            var newNode = new Node(
                                adjCell.position,
                                GetFacing(node.position, adjCell.position),
                                node.length + 1,
                                node
                            );
                            if (newNode.position == finish) return newNode;
                            newLeafNodes.Add(newNode);
                        }
                    }
                }
                leafNodes = newLeafNodes;
            }
            throw new System.Exception("Path not found!");
        }

        Direction GetFacing(Position from, Position to) {
            var diff = to - from;
            if (diff == Position.up) {
                return Direction.Up;
            } else if (diff == Position.down) {
                return Direction.Down;
            } else if (diff == Position.left) {
                return Direction.Left;
            } else {
                return Direction.Right;
            }
        }

        class Node {
            public Position position { get; }
            public Direction facing { get; }
            public int length { get; }
            public Node previousNode { get; }

            public Node(Position position, Direction facing, int length, Node previousNode) {
                this.position = position;
                this.facing = facing;
                this.length = length;
                this.previousNode = previousNode;
            }

            public IEnumerable<Node> Nodes() {
                Node currentNode = this;
                while (currentNode != null) {
                    yield return currentNode;
                    currentNode = currentNode.previousNode;
                }
            }
        }
    }    
}
