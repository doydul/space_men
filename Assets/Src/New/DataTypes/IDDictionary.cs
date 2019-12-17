using System.Collections.Generic;

namespace DataTypes {
    
    public class IDDictionary<T> {

        Dictionary<long, T> elements;
        
        public IDDictionary() {
            elements = new Dictionary<long, T>();
        }

        public long AddElement(T element) {
            var id = GenerateUniqueId();
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

        long GenerateUniqueId() {
            return System.DateTime.Now.ToFileTime() + UnityEngine.Random.Range(0, 1000);
        }
    }
}
