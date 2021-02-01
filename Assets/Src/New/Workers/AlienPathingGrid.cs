using System.Collections.Generic;
using System.Linq;
using System;

using Data;

namespace Workers {
    
    public class AlienPathingGrid {
        
        public static AlienPathingGrid instance { get; set; }
        
        public static void Calculate(GameState gameState) {
            instance = new AlienPathingGrid(gameState);
            instance.InitInstance();
        }
        
        GameState gameState;
        
        GridSquare[,] gridSquares;
        
        AlienPathingGrid(GameState gameState) {
            this.gameState = gameState;
        }
        
        void InitInstance() {
            var map = gameState.map;
            var checkedPositions = new List<Position>();
            gridSquares = new GridSquare[map.width, map.height];
            var iterators = gameState.GetActors().Where(actor => actor is SoldierActor).Select(soldier => {
                return new CellIterator(soldier.position, cell => !cell.isWall && !checkedPositions.Contains(cell.position)).Iterate(map).GetEnumerator();
            }).ToList();
            
            bool anyActiveIterators = true;
            while (anyActiveIterators) {
                anyActiveIterators = false;
                foreach (var iterator in iterators) {
                    if (iterator.MoveNext()) {
                        anyActiveIterators = true;
                        var node = iterator.Current;
                        var position = node.cell.position;
                        GridSquare nextSquare = null;
                        var facing = Direction.Up;
                        if (node.previousNode.cell != null) {
                            var nextPosition = node.previousNode.cell.position;
                            nextSquare = gridSquares[nextPosition.x, nextPosition.y];
                            facing = GetFacing(position, nextPosition);
                        }
                        
                        checkedPositions.Add(position);
                        gridSquares[position.x, position.y] = new GridSquare(
                            position: position,
                            nextSquare: nextSquare,
                            distanceToNearestSoldier: node.distanceFromStart,
                            facing: facing
                        );
                        if (nextSquare != null) nextSquare.previousSquare = gridSquares[position.x, position.y];
                    }
                }
            }
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

        public GridSquare GetSquare(Position position) {
            return gridSquares[position.x, position.y];
        }
        
        public class GridSquare {
            
            public Position position { get; }
            public GridSquare previousSquare { get; set; }
            public GridSquare nextSquare { get; }
            public int distanceToNearestSoldier { get; }
            public Direction facing { get; }
            
            public GridSquare(Position position, GridSquare nextSquare, int distanceToNearestSoldier, Direction facing) {
                this.position = position;
                this.nextSquare = nextSquare;
                this.distanceToNearestSoldier = distanceToNearestSoldier;
                this.facing = facing;
            }
        }
    }
}
