using Data;
using UnityEngine;

public class SpawnAliensPresenter : Presenter, IPresenter<SpawnAliensOutput> {

    public Map map;
  
    public static SpawnAliensPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(SpawnAliensOutput input) {
        foreach (var newAlien in input.newAliens) {
            InstantiateAlien(newAlien);
        }
    }

    Transform InstantiateAlien(Data.Alien newAlien) {
        var alienTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;
        var alien = alienTransform.GetComponent<Alien>() as Alien;
        // alien.FromData(Resources.Load<AlienData>("Aliens/" + newAlien.alienType));
        var spriteTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/" + newAlien.alienType.ToString() + "AlienSprite")) as Transform;
        spriteTransform.parent = alienTransform;
        spriteTransform.localPosition = Vector3.zero;
        alien.image = spriteTransform;
        alien.index = newAlien.index;
        alien.type = newAlien.alienType;

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
}

