using Data;
using UnityEngine;

public class MissionStartPresenter : Presenter, IPresenter<MissionStartOutput> {
  
    public static MissionStartPresenter instance { get; private set; }

    void Awake() {
        instance = this;
    }

    public Scripting scripting;
    public ShipEnergyDisplay shipEnergyDisplay;
    public Map map;
    
    public void Present(MissionStartOutput input) {
        foreach (var soldier in input.soldiers) {
            InstantiateSoldier(soldier, soldier.soldierId);
        }
        SetFog(input.fogs);
        AlienDeployer.instance.Iterate();
        RadarBlipController.instance.ShowRadarBlips();
        shipEnergyDisplay.Init(5);
        scripting.Trigger(Scripting.Event.OnMissionStart);
    }
    
    Soldier InstantiateSoldier(SoldierDisplayInfo soldierData, long index) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.index = index;
        soldier.armour = Armour.Get(soldierData.armourName.ToString());
        soldier.weapon = Weapon.Get(soldierData.weaponName.ToString());
        soldier.maxAmmo = soldierData.maxAmmo;
        soldier.exp = soldierData.exp;
        soldier.maxHealth = soldierData.maxHealth;
        soldier.health = soldierData.health;

        Map.instance.GetTileAt(new Vector2(soldierData.position.x, soldierData.position.y)).SetActor(trans);
        return soldier;
    }

    void SetFog(Fog[] fogs) {
        foreach (var tile in map.EnumerateTiles()) {
            tile.RemoveFoggy();
        }
        foreach (var fog in fogs) {
            map.GetTileAt(new Vector2(fog.position.x, fog.position.y)).SetFoggy();
        }
    }
}

