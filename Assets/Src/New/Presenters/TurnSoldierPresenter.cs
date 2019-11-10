using Data;

public class TurnSoldierPresenter : Presenter, IPresenter<TurnSoldierOutput> {
  
    public static TurnSoldierPresenter instance { get; private set; }

    public Map map;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(TurnSoldierOutput input) {
        (map.GetActorByIndex(input.index) as Soldier).TurnTo(ConvertDirection(input.newFacing));
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

