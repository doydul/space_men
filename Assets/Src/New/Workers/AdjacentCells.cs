using Data;
using System.Collections.Generic;

namespace Workers {
    
    public class AdjacentCells {
        
        Data.Map map;
        
        public AdjacentCells(Data.Map map) {
            this.map = map;
        }
        
        public IEnumerable<Cell> Iterate(Position position) {
            var x = position.x;
            var y = position.y;
            if (CellExists(x - 1, y)) yield return map.cells[x - 1, y];
            if (CellExists(x + 1, y)) yield return map.cells[x + 1, y];
            if (CellExists(x, y - 1)) yield return map.cells[x, y - 1];
            if (CellExists(x, y + 1)) yield return map.cells[x, y + 1];
        }
        
        bool CellExists(int x, int y) {
            if (x < 0 || y < 0) return false;
            if (x >= map.cells.GetLength(0) || y >= map.cells.GetLength(1)) return false;
            return true;
        }
    }
}
