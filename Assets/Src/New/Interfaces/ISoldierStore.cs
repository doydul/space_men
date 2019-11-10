using Data;

public interface ISoldierStore {
    
    ArmourStats GetArmourStats(string armourName);
    WeaponStats GetWeaponStats(string weaponName);
}
