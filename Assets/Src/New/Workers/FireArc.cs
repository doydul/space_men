using Data;
using UnityEngine;

namespace Workers {
    public class FireArc {

        [Dependency] GameState gameState;

        Position position;
        Direction facing;

        public FireArc(Position position, Direction facing) {
            this.position = position;
            this.facing = facing;
        }

        public bool WithinArcAndLOS(Position target) {
            return WithinArc(target) && WithinLOS(target);
        }

        public bool WithinArc(Position target) {
            return WithinSightArc(position, facing, target);
        }

        public bool WithinLOS(Position target) {
            return !LineOfSightBlocked(gameState.map, position, target);
        }

        bool WithinSightArc(Position shooterPosition, Direction shooterFacing, Position targetPosition) {
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

        bool LineOfSightBlocked(MapState map, Position shooterPosition, Position targetPosition) {
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

        float Blockage(MapState map, Position location) {
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