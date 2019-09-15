using UnityEngine;

namespace Data {

    public struct Position {
        
        public int x;
        public int y;
        
        public static Position up { get { return new Position(0, 1); } }
        public static Position right { get { return new Position(1, 0); } }
        public static Position down { get { return new Position(0, -1); } }
        public static Position left { get { return new Position(-1, 0); } }
        
        public int distance { get { 
            return Mathf.Abs(x) + Mathf.Abs(y);
        } }
        
        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }
        
        public static Position operator -(Position p1, Position p2) {  
            var temp = new Position();  
            temp.x = p1.x - p2.x;  
            temp.y = p1.y - p2.y;  
            return temp;  
        }
        
        public static bool operator ==(Position p1, Position p2) {
            return p1.x == p2.x && p1.y == p2.y;
        }
        
        public static bool operator !=(Position p1, Position p2) {
            return !(p1 == p2);
        }
    }
}
