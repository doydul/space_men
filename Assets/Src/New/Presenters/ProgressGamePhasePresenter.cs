using UnityEngine;
using TMPro;

using Data;

public class ProgressGamePhasePresenter : Presenter, IPresenter<ProgressGamePhaseOutput> {
  
    public Map map;
    public TMP_Text gamePhaseText;
    public GameObject turnButtonContainer;

    Data.GamePhase currentPhase;

    public static ProgressGamePhasePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ProgressGamePhaseOutput input) {
        if (currentPhase != input.currentPhase) {
            UpdateUI(input.currentPhase);
            UpdatePhaseText(input.currentPhase);
            currentPhase = input.currentPhase;
        }

        if (input.alienActions != null) {
            foreach (var action in input.alienActions) {
                if (action.type == AlienActionType.Move) {
                    MoveAlien(
                        GetAlienByIndex(action.index),
                        new Vector2(action.position.x, action.position.y),
                        ConvertDirection(action.facing)
                    );
                } else {
                    GetAlienByIndex(action.index).Face(new Vector2(action.position.x, action.position.y));
                    UnityEngine.Debug.Log(action.damage);
                    if (action.attackResult == AttackResult.Killed) {
                        var tile = map.GetTileAt(new Vector2(action.position.x, action.position.y));
                        tile.GetActor<Soldier>().Destroy();
                        tile.RemoveActor();
                    }
                }
            }
        }

        if (input.newAliens != null) {
            foreach (var newAlien in input.newAliens) {
                InstantiateAlien(newAlien);
            }
        }
    }

    void UpdateUI(Data.GamePhase gamePhase) {
        turnButtonContainer.SetActive(gamePhase == Data.GamePhase.Movement);
        foreach (var soldier in map.GetActors<Soldier>()) {
            if (gamePhase == Data.GamePhase.Movement) {
                soldier.StartMovementPhase();
            } else {
                soldier.StartShootingPhase();
            }
        }
    }

    void UpdatePhaseText(Data.GamePhase gamePhase) {
        if (gamePhase == Data.GamePhase.Movement) {
            gamePhaseText.text = "Movement Phase";
        } else {
            gamePhaseText.text = "Shooting Phase";
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
