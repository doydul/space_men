using UnityEngine;
using System.Collections.Generic;
using Workers;
using Data;
using System.Linq;

public class Test : MonoBehaviour {

    public MapState map;

    void Start() {
        MakeMap();
        // TestIterator();
        // TestLayerIterator();
        MakeExplosion();
    }

    void TestIterator() {
        var it = new CellIterator(new Position(3, 3), cell => true);
        foreach (var node in it.Iterate(map)) {
            var pos = node.cell.position;
            Debug.Log("(" + pos.x + ", " + pos.y + ")");
        }
    }

    void TestLayerIterator() {
        var it = new CellLayerIterator(new Position(3, 3), cell => true);
        foreach (var layer in it.Iterate(map)) {
            Debug.Log("XXXXXX " + layer.distanceFromStart);
            foreach (var node in layer.nodes) {
                var pos = node.cell.position;
                // Debug.Log("(" + pos.x + ", " + pos.y + ")");
            }
        }
    }

    void MakeMap() {
        map = new MapState();
        var cells = new Cell[7, 7];
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                cells[x, y] = new Cell { position = new Position(x, y) };
            }
        }
        map.Init(cells);
    }

    void MakeExplosion() {
        var result = GenerateExplosion(new Position(3, 3), 0, 5);
        Debug.Log("Result: ");
        foreach (var pos in result) {
            Debug.Log("(" + pos.x + ", " + pos.y + ")");
        }
    }

    Position[] GenerateExplosion(Position targetPosition, int accuracy, int blastSize) {
        var result = new List<Position>();
        var realPosition = targetPosition;

        var iterator = new CellLayerIterator(targetPosition, cell => !cell.isWall);
        int layerI = -4;
        foreach (var layer in iterator.Iterate(map)) {
            blastSize -= Mathf.Max(layerI, 0);
            foreach (var node in layer.nodes.OrderBy(x => Random.value).Take(blastSize)) {
                result.Add(node.cell.position);
                blastSize--;
            }
            if (blastSize <= 0) break;
            layerI += 2;
        }
        return result.ToArray();
    }
}
