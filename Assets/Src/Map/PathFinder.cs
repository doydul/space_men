using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder {

  private IPathable _grid;
  private Vector2 _start;
  private List<Vector2> _targets;

  private List<Vector2> traversed_squares;
  private List<Node> end_nodes;

  public PathFinder(IPathable grid, Vector2 start, List<Vector2> targets) {
    _grid = grid;
    _start = start;
    _targets = targets;

    // Dictionary would be more efficient
    traversed_squares = new List<Vector2>();
    end_nodes = new List<Node>();
  }
  
  public PathFinder(IPathable grid, Vector2 start) {
      _grid = grid;
      _start = start;

      // Dictionary would be more efficient
      traversed_squares = new List<Vector2>();
      end_nodes = new List<Node>();
  }
  
  public List<Vector2> FindPath(List<Vector2> targets) {
    _targets = targets;
    return FindPath();
  }

  public List<Vector2> FindPath() {
    if (HuristicFor(_start) <= 1) return new List<Vector2> { _start };
    
    end_nodes.Add(new Node(_start, null, 0, 0));
    traversed_squares.Add(_start);
    
    while(end_nodes.Count > 0) {
      var node = best_node();
      end_nodes.Remove(node);

      foreach (var square in adjacent_squares(node.square)) {
        int huristic = HuristicFor(square);
        var new_node = new Node(square, node, node.path_length + 1, huristic);
        if (new_node.huristic == 1) {
          return expand_path(new_node);
        }
        end_nodes.Add(new_node);
        traversed_squares.Add(square);
      }
    }

    return null;
  }
  
  private int HuristicFor(Vector2 square) {
      int huristic = 999;
      foreach (var target in _targets) {
        float distance = Mathf.Abs(target.x - square.x) + Mathf.Abs(target.y - square.y);
        if (distance < huristic) {
          huristic = (int)distance;
        }
      }
      return huristic;
  }

  private Node best_node() {
    Node result = null;
    foreach (var node in end_nodes) {
      if (result == null || node.weight < result.weight) {
        result = node;
      }
    }
    return result;
  }

  private List<Vector2> expand_path(Node node) {
    if (node == null) return null;
    List<Vector2> result = new List<Vector2>();
    result.Add(node.square);

    var next_node = node.previous_node;
    while (next_node != null) {
      result.Add(next_node.square);
      next_node = next_node.previous_node;
    }

    result.Reverse();
    return result;
  }

  private List<Vector2> adjacent_squares(Vector2 position) {
    var result = new List<Vector2>();
    if (square_empty(position.x, position.y + 1) && !traversed_squares.Contains(new Vector2(position.x, position.y + 1))) {
      result.Add(new Vector2(position.x, position.y + 1));
    }
    if (square_empty(position.x, position.y - 1) && !traversed_squares.Contains(new Vector2(position.x, position.y - 1))) {
      result.Add(new Vector2(position.x, position.y - 1));
    }
    if (square_empty(position.x + 1, position.y) && !traversed_squares.Contains(new Vector2(position.x + 1, position.y))) {
      result.Add(new Vector2(position.x + 1, position.y));
    }
    if (square_empty(position.x - 1, position.y) && !traversed_squares.Contains(new Vector2(position.x - 1, position.y))) {
      result.Add(new Vector2(position.x - 1, position.y));
    }
    return result;
  }

  private bool square_empty(float x, float y) {
    return _grid.LocationPathable(new Vector2(x, y));
  }

  private class Node {

    private Vector2 _square;
    private Node _previous_node;
    private int _path_length;
    private int _huristic;

    public Vector2 square {
      get {
        return _square;
      }
    }

    public Node previous_node {
      get {
        return _previous_node;
      }
    }

    public int weight {
      get {
        return _path_length + _huristic;
      }
    }

    public int path_length {
      get {
        return _path_length;
      }
    }

    public int huristic {
      get {
        return _huristic;
      }
    }

    public Node(Vector2 square, Node previous_node, int path_length, int huristic) {
      _square = square;
      _previous_node = previous_node;
      _path_length = path_length;
      _huristic = huristic;
    }
  }
}
