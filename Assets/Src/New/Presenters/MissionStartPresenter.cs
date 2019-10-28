using Data;
using UnityEngine;

public class MissionStartPresenter : Presenter, IPresenter<MissionStartOutput> {
  
    public static MissionStartPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(MissionStartOutput input) {
        SoldierDataHack.Init();
        foreach (var soldier in input.soldiers) {
            SoldierDataHack.soldiers.Add(
                InstantiateSoldier(soldier, soldier.index)
            );
        }
        FogController.instance.Recalculate();
        AlienDeployer.instance.Iterate();
        RadarBlipController.instance.ShowRadarBlips();
    }
    
    Soldier InstantiateSoldier(Data.Soldier soldierData, long index) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.index = index;
        soldier.armour = Armour.Get(soldierData.armourType.ToString());
        soldier.weapon = Weapon.Get(soldierData.weaponName);
        soldier.exp = soldierData.exp;

        Map.instance.GetTileAt(new Vector2(soldierData.position.x, soldierData.position.y)).SetActor(trans);
        return soldier;
    }
}

