using UnityEngine;
using System.Collections.Generic;

public class Path {
    
    private List<Vector2> nodes;
    
    public int Count { get { return nodes.Count; } }
    
    public Path(List<Vector2> nodes) {
        this.nodes = nodes;
    }
    
    public List<Vector2> First(int number) {
        List<Vector2> slice = null;
        if (number > nodes.Count) {
          slice = new List<Vector2>(nodes);
        } else {
          slice = nodes.GetRange(0, number);
        }
        return slice;
    }
    
    public Vector2 First() {
        return nodes[0];
    }
    
    public Vector2 Last() {
        return nodes[nodes.Count - 1];
    }
    
    public List<Vector2> Last(int number) {
        List<Vector2> slice = null;
        if (number > nodes.Count) {
          slice = new List<Vector2>(nodes);
        } else {
          slice = nodes.GetRange(nodes.Count - number - 1, number);
        }
        return slice;
    }
    
    public Vector2 NthFromEnd(int number) {
        return Last(number)[0];
    }
    
    public List<Vector2> FirstReverse(int number) {
        var result = First(number);
        result.Reverse();
        return result;
    }
}