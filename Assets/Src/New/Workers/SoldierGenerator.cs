using Data;

namespace Workers {
    
    public static class SoldierGenerator {
        
        public static Builder Default() {
            return new Builder(new SoldierActor {
                health = new Health(15),
                armourName = "Basic",
                weaponName = "Assault Rifle",
                baseMovement = 4,
                totalMovement = 8
            });
        }
        
        public static Builder WithWeapon(string weaponName) {
            return Default().WithWeapon(weaponName);
        }
        
        public static Builder WithArmour(string armourName) {
            return Default().WithArmour(armourName);
        }

        public static Builder WithMaxHealth(int maxHealth) {
            return Default().WithMaxHealth(maxHealth);
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
            
            public Builder WithArmour(string armourName) {
                var newData = data;
                newData.armourName = armourName;
                data = newData;
                return this;
            }

            public Builder WithMaxHealth(int maxHealth) {
                data.health = new Health(maxHealth);
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
