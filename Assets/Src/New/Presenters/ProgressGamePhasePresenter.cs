using Data;
using UnityEngine;

public class ProgressGamePhasePresenter : Presenter, IPresenter<ProgressGamePhaseOutput> {
  
    public Map map;

    public static ProgressGamePhasePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ProgressGamePhaseOutput input) {
        UnityEngine.Debug.Log("GAME PHASE PROGRESSED");
        UnityEngine.Debug.Log("Phase " + input.currentPhase);
        if (input.newAliens != null) { 
            foreach (var alien in input.newAliens) {
                UnityEngine.Debug.Log("New Alien:");
                UnityEngine.Debug.Log(alien.alienType);
            }
        }
        if (input.alienActions != null) {
            foreach (var action in input.alienActions) {
                if (action.type == AlienActionType.Move) {
                    MoveAlien(
                        GetAlienByIndex(action.index),
                        new Vector2(action.position.x, action.position.y),
                        ConvertDirection(action.facing)
                    );
                }
            }
        }
        UnityEngine.Debug.Log("--------------------");

        if (input.newAliens != null) {
            foreach (var newAlien in input.newAliens) {
                InstantiateAlien(newAlien);
            }
        }
    }

    Transform InstantiateAlien(Data.Alien newAlien) {
        var alienTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;
        var alien = alienTransform.GetComponent<Alien>() as Alien;
        alien.FromData(Resources.Load<AlienData>("Aliens/" + newAlien.alienType.ToString()));
        var spriteTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + newAlien.alienType.ToString() + "AlienSprite")) as Transform;
        spriteTransform.parent = alienTransform;
        spriteTransform.localPosition = Vector3.zero;
        alien.image = spriteTransform;
        alien.index = newAlien.index;

        alien.TurnTo(ConvertDirection(newAlien.facing));
        map.GetTileAt(new Vector2(newAlien.position.x, newAlien.position.y)).SetActor(alienTransform);
        return alienTransform;
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
    
    void MoveAlien(Alien alien, Vector2 destination, Actor.Direction direction) {
        alien.MoveTo(map.GetTileAt(destination));
        alien.TurnTo(direction);
    }
    
    Alien GetAlienByIndex(long index) {
        foreach (var alien in map.GetActors<Alien>()) {
            if (alien.index == index) return alien;
        }
        throw new System.Exception("Alien could not be found");
    }
}
