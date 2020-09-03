namespace Data {
    
    public struct OpenArmouryOutput {
        
        public SoldierDisplayInfo[] squadSoldiers;
        public int credits;
    }

    public class SoldierDisplayInfo {
        
        public bool empty;
        public long soldierId;
        public string weaponName;
        public string armourName;
    }
}
