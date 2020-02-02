namespace Data {
    
    public struct OpenArmouryOutput {
        
        public SoldierDisplayInfo[] squadSoldiers;
        public int credits;
    }

    public class SoldierDisplayInfo {
        
        public long soldierId;
        public string weaponName;
        public string armourName;
    }
}
