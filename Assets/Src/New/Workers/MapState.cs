using Data;

using System.Collections.Generic;
using System.Linq;

namespace Workers {

    public class MapState {

        public CellType[,] cells;
        
        public int width { get { return cells.GetLength(0); } }
        public int height { get { return cells.GetLength(1); } }
        
        public Position[] spawners { get; private set; }
        public Position[] alienSpawners { get; private set; }

        public void Init(Cell[,] cells) {
            SetCells(cells);
            InitSpawners();
            InitAlienSpawners();
        }

        public CellType GetCell(Position position) {
            return cells[position.x, position.y];
        }
        
        public CellType GetCell(int x, int y) {
            return cells[x, y];
        }

        public IEnumerable<CellType> GetAllCells() {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    yield return cells[x, y];
                }
            }
        }

        public void UpdateCell(Position position, CellType updatedCell) {
            cells[position.x, position.y] = updatedCell;
        }
        
        void SetCells(Cell[,] cells) {
            this.cells = new CellType[cells.GetLength(0), cells.GetLength(1)];
            for (int i = 0; i < cells.GetLength(0); i++) {
                for (int j = 0; j < cells.GetLength(1); j++) {
                    this.cells[i, j] = CellType.FromValueType(cells[i, j]);
                }
            }
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
