using Data;

namespace Workers {
    
    public static class SoldierGenerator {
        
        public static Builder Default() {
            return new Builder(new SoldierActor {
                health = new Health(86),
                armourType = ArmourType.Basic,
                weaponName = "Assault Rifle",
                baseMovement = 4,
                totalMovement = 8
            });
        }
        
        public static Builder WithWeapon(string weaponName) {
            return Default().WithWeapon(weaponName);
        }
        
        public static Builder WithArmour(ArmourType armourType) {
            return Default().WithArmour(armourType);
        }
        
        public static Builder At(Data.Position position) {
            return Default().At(position);
        }
        
        public class Builder {
            
            SoldierActor data;
            
            public Builder(SoldierActor data) {
                this.data = data;
            }
            
            public Builder WithWeapon(string weaponName) {
                var newData = data;
                newData.weaponName = weaponName;
                data = newData;
                return this;
            }
            
            public Builder WithArmour(ArmourType armourType) {
                var newData = data;
                newData.armourType = armourType;
                data = newData;
                return this;
            }
            
            public Builder At(Data.Position position) {
                var newData = data;
                newData.position = position;
                data = newData;
                return this;
            }
            
            public SoldierActor Build() {
                return data;
            }
        }
    }
}
