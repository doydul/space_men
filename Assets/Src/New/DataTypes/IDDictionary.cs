using System.Collections.Generic;

namespace DataTypes {
    
    public class IDDictionary<T> {

        Dictionary<long, T> elements;
        int currentFrame;
        int idTicker;
        
        public IDDictionary() {
            elements = new Dictionary<long, T>();
        }

        public int Count => elements.Count;

        public long AddElement(T element) {
            var id = GenerateUniqueId();
            elements.Add(id, element);
            return id;
        }

        public long AddElement(T element, long id) {
            elements.Add(id, element);
            return id;
        }

        public T GetElement(long id) {
            return elements[id];
        }
        
        public IEnumerable<T> GetElements() {
            return new List<T>(elements.Values);
        }

        public void RemoveElement(long id) {
            elements.Remove(id);
        }

        public long GenerateUniqueId() {
            if (currentFrame != UnityEngine.Time.frameCount) {
                currentFrame = UnityEngine.Time.frameCount;
                idTicker = 0;
            }
            idTicker++;
            return (System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond) * 100 + idTicker;
        }
    }
}
