using Data;
using UnityEngine;

public class MissionStartPresenter : Presenter, IPresenter<MissionStartOutput> {
  
    public static MissionStartPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(MissionStartOutput input) {
        foreach (var soldier in input.soldiers) {
            InstantiateSoldier(soldier, soldier.index);
        }
        FogController.instance.Recalculate();
        AlienDeployer.instance.Iterate();
        RadarBlipController.instance.ShowRadarBlips();
    }
    
    Soldier InstantiateSoldier(Data.Soldier soldierData, long index) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.index = index;
        soldier.armour = Armour.Get(soldierData.armourName.ToString());
        soldier.weapon = Weapon.Get(soldierData.weaponName.ToString());
        soldier.exp = soldierData.exp;
        soldier.maxHealth = soldierData.maxHealth;
        soldier.health = soldierData.health;

        Map.instance.GetTileAt(new Vector2(soldierData.position.x, soldierData.position.y)).SetActor(trans);
        return soldier;
    }
}

