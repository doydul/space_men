using Data;
using UnityEngine;

public class ExecuteShipAbilityPresenter : Presenter, IPresenter<ExecuteShipAbilityOutput> {
  
    public static ExecuteShipAbilityPresenter instance { get; private set; }

    public UIData uiData;
    public Map map;
    public ShipEnergyDisplay shipEnergyDisplay;
    public MapHighlighter mapHighlighter;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ExecuteShipAbilityOutput input) {
        shipEnergyDisplay.Drain();

        if (input.shipAbilityOutput.newSoldier.HasValue) {
            InstantiateSoldier(input.shipAbilityOutput.newSoldier.Value);
        }

        uiData.ClearSelection();
        mapHighlighter.ClearHighlights();
    }

    Soldier InstantiateSoldier(Data.Soldier soldierData) {
        var trans = Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.index = soldierData.index;
        soldier.armour = Armour.Get(soldierData.armourName.ToString());
        soldier.weapon = Weapon.Get(soldierData.weaponName.ToString());
        soldier.exp = soldierData.exp;
        soldier.maxHealth = soldierData.maxHealth;
        soldier.health = soldierData.health;
        soldier.TurnTo((Actor.Direction)soldierData.facing);

        map.GetTileAt(new Vector2(soldierData.position.x, soldierData.position.y)).SetActor(trans);
        return soldier;
    }
}

