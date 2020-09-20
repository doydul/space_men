using Data;
using UnityEngine;

public class ExecuteShipAbilityPresenter : Presenter, IPresenter<ExecuteShipAbilityOutput> {
  
    public static ExecuteShipAbilityPresenter instance { get; private set; }

    public UIData uiData;
    public Map map;
    public ShipEnergyDisplay shipEnergyDisplay;
    public MapHighlighter mapHighlighter;
    public Transform ammoCratePrefab;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ExecuteShipAbilityOutput input) {
        shipEnergyDisplay.SetLevel(input.newShipEnergyLevel);

        if (input.shipAbilityOutput.newSoldier != null) {
            InstantiateSoldier(input.shipAbilityOutput.newSoldier);
        }
        if (input.shipAbilityOutput.newAmmoCrate.HasValue) {
            InstantiateCrate(input.shipAbilityOutput.newAmmoCrate.Value);
        }

        uiData.ClearSelection();
        mapHighlighter.ClearHighlights();
    }

    Soldier InstantiateSoldier(SoldierDisplayInfo soldierData) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.index = soldierData.soldierId;
        soldier.armour = Armour.Get(soldierData.armourName.ToString());
        soldier.weapon = Weapon.Get(soldierData.weaponName.ToString());
        soldier.exp = soldierData.exp;
        soldier.maxHealth = soldierData.maxHealth;
        soldier.health = soldierData.health;
        soldier.TurnTo((Actor.Direction)soldierData.facing);

        map.GetTileAt(new Vector2(soldierData.position.x, soldierData.position.y)).SetActor(trans);
        return soldier;
    }

    void InstantiateCrate(Data.Crate crateData) {
        var trans = Instantiate(ammoCratePrefab) as Transform;
        var tile = map.GetTileAt(new Vector2(crateData.position.x, crateData.position.y));
        tile.SetActor(trans, true);
    }
}

