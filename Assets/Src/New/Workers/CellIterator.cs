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
            var leafNodes = new List<Node> { new Node { cell = map.GetCell(start) } };
            var checkedPositions = new List<Position> { start };
            yield return new Node { cell = map.GetCell(start) };
            
            int distance = 1;
            while (leafNodes.Count > 0) {
                var newLeafNodes = new List<Node>();
                foreach (var leafNode in leafNodes) {
                    foreach (var cell in iterator.Iterate(leafNode.cell.position)) {
                        if (!checkedPositions.Contains(cell.position) && filterCondition(cell)) {
                            var newNode = new Node {
                                cell = cell,
                                previousNode = leafNode,
                                distanceFromStart = distance
                            };
                            yield return newNode;
                            newLeafNodes.Add(newNode);
                            checkedPositions.Add(cell.position);
                        }
                    }
                }
                leafNodes = newLeafNodes;
                distance++;
            }
        }
        
        public class Node {
            
            public CellType cell;
            public Node previousNode;
            public int distanceFromStart;

            public Node[] GetPath() {
                var result = new List<Node>() { this };
                var current = previousNode;
                while (current != null) {
                    result.Add(current);
                    current = current.previousNode;
                }
                result.Reverse();
                return result.ToArray();
            }
        }
    }
}