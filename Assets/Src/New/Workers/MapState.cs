using Data;

using System.Collections.Generic;

namespace Workers {

    public class MapState {

        public Cell[,] cells;
        
        public int width { get { return cells.GetLength(0); } }
        public int height { get { return cells.GetLength(1); } }
        
        public Position[] spawners { get; private set; }
        public Position[] alienSpawners { get; private set; }

        public void Init(Cell[,] cells) {
            this.cells = cells;
            InitSpawners();
            InitAlienSpawners();
        }

        public Cell GetCell(Position position) {
            return cells[position.x, position.y];
        }
        
        public Cell GetCell(int x, int y) {
            return cells[x, y];
        }

        public void UpdateCell(Position position, Cell updatedCell) {
            cells[position.x, position.y] = updatedCell;
        }
        
        void InitSpawners() {
            var spawnerList = new List<Position>();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (cells[i, j].isSpawnPoint) spawnerList.Add(cells[i, j].position);
                }
            }
            spawners = spawnerList.ToArray();
        }
        
        void InitAlienSpawners() {
            var spawnerList = new List<Position>();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (cells[i, j].isAlienSpawnPoint) spawnerList.Add(cells[i, j].position);
                }
            }
            alienSpawners = spawnerList.ToArray();
        }
    }    
}
