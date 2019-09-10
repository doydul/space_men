using System.Collections.Generic;

namespace Workers {
    
    public class ActorStore<T> {

        int currentIndex = 0;
        Dictionary<int, T> actors;
        
        public ActorStore() {
            actors = new Dictionary<int, T>();
        }

        public int AddActor(T actor) {
            actors.Add(currentIndex, actor);
            currentIndex++;
            return currentIndex - 1;
        }

        public T GetActor(int index) {
            return actors[index];
        }

        public void RemoveActor(int index) {
            actors.Remove(index);
        }

        public void UpdateActor(int index, T updatedActor) {
            actors[index] = updatedActor;
        }
        
        public int[] GetIndexes() {
            var result = new int[actors.Keys.Count];
            int index = 0;
            foreach (var key in actors.Keys) {
                result[index] = key;
                index++;
            }
            return result;
        }
    }
}
