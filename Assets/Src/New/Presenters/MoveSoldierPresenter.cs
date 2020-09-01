using UnityEngine;

using Data;

public class MoveSoldierPresenter : Presenter, IPresenter<MoveSoldierOutput> {
  
    public static MoveSoldierPresenter instance { get; private set; }

    public UIData uiData;
    public MapController mapController;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }

    public Map map;
    
    public void Present(MoveSoldierOutput input) {
        var tile = map.GetTileAt(new Vector2(input.newPosition.x, input.newPosition.y));
        var soldier = GetSoldierByIndex(input.soldierIndex);
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        if (input.movementType == MovementType.Running) {
            soldierUI.SetMoved();
        } else if (input.movementType == MovementType.Sprinting) {
            soldierUI.SetSprinted();
        }
        soldier.tile.RemoveActor();
        tile.SetActor(soldier.transform);
        soldier.TurnTo(ConvertDirection(input.newFacing));
        uiData.selectedTile = tile;
        SetFog(input.newFogs);
        mapController.DisplayActions(input.soldierIndex);
        TriggerTraversedTileEvents(input);
        scripting.Trigger(Scripting.Event.OnMoveSoldier);
    }

    void TriggerTraversedTileEvents(MoveSoldierOutput input) {
        foreach (var position in input.traversedCells) {
            var tile = map.GetTileAt(new Vector2(position.x, position.y));
            tile.SendMessage("OnTraverse", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    Soldier GetSoldierByIndex(long index) {
        foreach (var soldier in map.GetActors<Soldier>()) {
            if (soldier.index == index) return soldier;
        }
        throw new System.Exception("Soldier could not be found");
    }

    Actor.Direction ConvertDirection(Data.Direction direction) {
        if (direction == Data.Direction.Up) {
            return Actor.Direction.Up;
        } else if (direction == Data.Direction.Down) {
            return Actor.Direction.Down;
        } else if (direction == Data.Direction.Left) {
            return Actor.Direction.Left;
        } else {
            return Actor.Direction.Right;
        }
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

