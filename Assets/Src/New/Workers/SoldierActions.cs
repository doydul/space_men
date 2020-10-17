using UnityEngine;
using System.Collections.Generic;

using Data;

namespace Workers {
    public static class SoldierActions {

        public static ActorAction[] ShootingActionsFor(GameState gameState, SoldierDecorator soldier) {
            var result = new List<ActorAction>();
            foreach (var alien in Aliens.Iterate(gameState)) {
                if (
                    soldier.CanShoot() &&
                    SoldierActions.WithinSightArc(soldier.position, soldier.facing, alien.position) &&
                    !SoldierActions.LineOfSightBlocked(gameState.map, soldier.position, alien.position)
                ) {
                    result.Add(new ActorAction {
                        index = soldier.uniqueId,
                        type = ActorActionType.Shoot,
                        target = alien.position,
                        actorTargetIndex = alien.uniqueId
                    });
                }
            }
            return result.ToArray();
        }

        static bool WithinSightArc(Position shooterPosition, Direction shooterFacing, Position targetPosition) {
            var distance = shooterPosition - targetPosition;
            if (shooterFacing == Direction.Up) {
                return distance.y < 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
            } else if (shooterFacing == Direction.Down) {
                return distance.y > 0 && Mathf.Abs(distance.x) <= Mathf.Abs(distance.y);
            } else if (shooterFacing == Direction.Left) {
                return distance.x > 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
            } else {
                return distance.x < 0 && Mathf.Abs(distance.y) <= Mathf.Abs(distance.x);
            }
        }

        static bool LineOfSightBlocked(MapState map, Position shooterPosition, Position targetPosition) {
            if (map.GetCell(targetPosition).isFoggy) return true;
            float blockage = 0f;
            var delta = targetPosition - shooterPosition;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                var ratio = (float)delta.y / Mathf.Abs(delta.x);
                for (int i = 0; i < Mathf.Abs(delta.x) - 0.1f; i++) {
                    var location = new Position(shooterPosition.x + i * (int)Mathf.Sign(delta.x), Mathf.RoundToInt(shooterPosition.y + ratio * i));
                    if (location != shooterPosition) blockage += Blockage(map, location);
                }
            } else {
                var ratio = (float)delta.x / Mathf.Abs(delta.y);
                for (int i = 0; i < Mathf.Abs(delta.y) - 0.1f; i++) {
                    var location = new Position(Mathf.RoundToInt(shooterPosition.x + ratio * i), shooterPosition.y + i * (int)Mathf.Sign(delta.y));
                    blockage += Blockage(map, location);
                }
            }
            return blockage >= 1;
        }

        static float Blockage(MapState map, Position location) {
            var cell = map.GetCell(location);
            if (cell.isWall) {
                return 1;
            } else if (cell.actor.isSoldier) {
                return 0.35f;
            } else if (cell.actor.isAlien) {
                return 0.15f;
            }
            return 0;
        }
    }
}