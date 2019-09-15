using System.Collections.Generic;
using System;

using Data;

namespace Workers {
    
    public class CellIterator {
        
        Position start;
        Func<CellType, bool> filterCondition;
        
        public CellIterator(Position start, Func<CellType, bool> filterCondition) {
            this.start = start;
            this.filterCondition = filterCondition;
        }
        
        public IEnumerable<Node> Iterate(Workers.MapState map) {
            var iterator = new AdjacentCells(map);
            var leafCells = new List<CellType> { map.GetCell(start) };
            var checkedPositions = new List<Position> { start };
            yield return new Node { cell = map.GetCell(start) };
            
            int distance = 0;
            while (leafCells.Count > 0) {
                var newLeafCells = new List<CellType>();
                foreach (var leafCell in leafCells) {
                    foreach (var cell in iterator.Iterate(leafCell.position)) {
                        if (!checkedPositions.Contains(cell.position) && filterCondition(cell)) {
                            yield return new Node {
                                cell = cell,
                                previousCell = leafCell,
                                distanceFromStart = distance
                            };
                            newLeafCells.Add(cell);
                            checkedPositions.Add(cell.position);
                        }
                    }
                }
                leafCells = newLeafCells;
                distance++;
            }
        }
        
        public struct Node {
            
            public CellType cell;
            public CellType previousCell;
            public int distanceFromStart;
        }
    }
}
