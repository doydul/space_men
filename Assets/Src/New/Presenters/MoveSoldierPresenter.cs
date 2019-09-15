using UnityEngine;

using Data;

public class MoveSoldierPresenter : Presenter, IPresenter<MoveSoldierOutput> {
  
    public static MoveSoldierPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public Map map;
    
    public void Present(MoveSoldierOutput input) {
        UnityEngine.Debug.Log(input);
        var soldier = GetSoldierByIndex(input.soldierIndex);
        soldier.tile.RemoveActor();
        map.GetTileAt(new Vector2(input.newPosition.x, input.newPosition.y)).SetActor(soldier.transform);
        soldier.TurnTo(ConvertDirection(input.newFacing));
        FogController.instance.Recalculate();
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
}

