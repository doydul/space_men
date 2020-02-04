using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

using Data;

namespace Workers {
    
    public class CellLayerIterator {
        
        Position start;
        Func<CellType, bool> filterCondition;
        
        public CellLayerIterator(Position start, Func<CellType, bool> filterCondition) {
            this.start = start;
            this.filterCondition = filterCondition;
        }
        
        public IEnumerable<Layer> Iterate(Workers.MapState map) {
            var iterator = new CellIterator(start, filterCondition);
            int distance = 0;
            
            var layerNodes = new List<CellIterator.Node>();
            foreach (var node in iterator.Iterate(map)) {
                if (node.distanceFromStart == distance) {
                    layerNodes.Add(node);
                } else if (node.distanceFromStart == distance + 1) {
                    yield return new Layer {
                        distanceFromStart = distance,
                        nodes = layerNodes.ToArray()
                    };
                    distance++;
                    layerNodes = new List<CellIterator.Node> { node };
                } else {
                    throw new Exception("Iteration ocurring in wrong order!");
                }
            }
            if (layerNodes.Any()) {
                yield return new Layer {
                    distanceFromStart = distance,
                    nodes = layerNodes.ToArray()
                };
            }
        }

        public struct Layer {

            public int distanceFromStart;
            public CellIterator.Node[] nodes;
        }
    }
}
