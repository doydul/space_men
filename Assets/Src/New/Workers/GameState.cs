namespace Workers {
    
    public class GameState {

        public IMapStore mapStore { private get; set; }

        public MapState map { get; private set; }
        
        ActorStore<Data.Soldier> soldiers;
        ActorStore<Data.Alien> aliens;

        public void Init() {
            soldiers = new ActorStore<Data.Soldier>();
            aliens = new ActorStore<Data.Alien>();

            map = new MapState();
            map.Init(mapStore.GetMap());
        }
        
        public int AddSoldier(Data.Soldier soldier) {
            return soldiers.AddActor(soldier);
        }
        
        public void UpdateSoldier(int index, Data.Soldier soldier) {
            soldiers.UpdateActor(index, soldier);
        }
        
        public int[] GetSoldierIndexes() {
            return soldiers.GetIndexes();
        }

        public Data.Soldier GetSoldier(int index) {
            return soldiers.GetActor(index);
        }
        
        public int AddAlien(Data.Alien alien) {
            return aliens.AddActor(alien);
        }
        
        public void UpdateAlien(int index, Data.Alien alien) {
            aliens.UpdateActor(index, alien);
        }
        
        public int[] GetAlienIndexes() {
            return aliens.GetIndexes();
        }
        
        public Data.Alien GetAlien(int index) {
            return aliens.GetActor(index);
        }
    }
}
