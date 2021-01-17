using System;
using System.Collections.Generic;
using System.Linq;

namespace Data { 
  
    public abstract class Actor {

        public Position position { get; set; }
        public Health health { get; set; }
        public long uniqueId { get; set; }
        public Direction facing { get; set; }
        
        public virtual bool exists => true;
        public virtual bool isSoldier => false;
        public virtual bool isAlien => false;
        public virtual bool isCrate => false;
        public virtual bool isFlame => false;

        List<object> arbitraryData;

        public void SetUniqueId(long id) {
            if (uniqueId != 0) throw new System.Exception("Unique ID can only be set once");
            uniqueId = id;
        }

        public void SetData(object datum) {
            if (arbitraryData == null) arbitraryData = new List<object>();
            var currentData = OfType(datum.GetType());
            foreach (var obj in currentData) {
                arbitraryData.Remove(obj);
            }
            arbitraryData.Add(datum);
        }

        public void AddData(object datum) {
            if (arbitraryData == null) arbitraryData = new List<object>();
            arbitraryData.Add(datum);
        }

        public T GetData<T>() {
            if (arbitraryData == null) arbitraryData = new List<object>();
            var currentData = arbitraryData.OfType<T>();
            if (currentData.Count() > 0) {
                return currentData.First();
            }
            return default(T);
        }

        public T[] GetAllData<T>() {
            if (arbitraryData == null) arbitraryData = new List<object>();
            return arbitraryData.OfType<T>().ToArray();
        }

        public void EraseData<T>() {
            foreach (var element in GetAllData<T>()) {
                arbitraryData.Remove(element);
            }
        }

        object[] OfType(Type type) {
            return arbitraryData.Where(obj => type.IsInstanceOfType(obj)).ToArray();
        }
    }
}
