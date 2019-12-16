using Workers;

namespace Data {

    public class WeaponReward : IReward {

        string weaponName;

        public WeaponReward(string weaponName) {
            this.weaponName = weaponName;
        }

        public void Grant(MetaGameState metaGameState) {
            metaGameState.metaItems.Add(new MetaWeapon {
                name = weaponName
            });
        }
    }
}