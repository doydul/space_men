using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {

    enum Facing {
        North,
        South,
        West,
        East
    }

    static Facing[] Facings = { Facing.North, Facing.South, Facing.West, Facing.East };

    class Element {

    }

    class Corridor : Element {
        public int length;
        public Facing direction;
        public Element northWestEnd;
        public Element SouthEastEnd;
    }

    class Room : Element {
        
    }

    class Cursor {
        public int x;
        public int y;

        public Cursor(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void Move(Facing direction) {
            if (direction == Facing.North) {
                y -= 1;
            } else if (direction == Facing.South) {
                y += 1;
            } else if (direction == Facing.East) {
                x += 1;
            } else if (direction == Facing.West) {
                x -= 1;
            }
        }
    }
    
    public MapLayout Generate() {
        var corridors = new List<Corridor>();
        for (int i = 0; i < 5; i++) {
            var corridor = new Corridor {
                length = Random.Range(2, 5),
                direction = Facings.Sample()
            };
            corridors.Add(corridor);
        }
        var walls = new MapLayout(20, 20);
        var cursor = new Cursor(10, 10);
        walls.SetWallAt(false, cursor.x, cursor.y);
        foreach (var corridor in corridors) {
            for (int i = 0; i < corridor.length; i++) {
                cursor.Move(corridor.direction);
                walls.SetWallAt(false, cursor.x, cursor.y);
            }
        }
        return walls;
    }
}

public class MapLayout {
    public List<List<bool>> walls;

    public MapLayout(int width, int height) {
        walls = new();
        for (int x = 0; x < width; x++) {
            walls.Add(new List<bool>(height));
            for (int y = 0; y < height; y++) {
                walls[x].Add(true);
            }
        }
    }

    public void SetWallAt(bool wall, int x, int y) {
        walls[x][y] = wall;
    }
}