using Data;

public class CollectAmmoPresenter : Presenter, IPresenter<CollectAmmoOutput> {
  
    public static CollectAmmoPresenter instance { get; private set; }

    public MapController mapController;
    public Scripting scripting;
    public Map map;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(CollectAmmoOutput input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        soldier.ammo = input.newAmmoCount;
        if (input.remainingCrateSupplies <= 0) {
            var tile = map.GetTileAt(soldier.gridLocation);
            var crate = tile.backgroundActor.GetComponent<Actor>();
            tile.RemoveBackgroundActor();
            crate.Die();
        }
        scripting.Trigger(Scripting.Event.OnCollectAmmo);
        mapController.DisplayActions(input.soldierIndex);
    }
}

