using UnityEngine;
using TMPro;
using System.Collections;

using Data;

public class ProgressGamePhasePresenter : Presenter, IPresenter<ProgressGamePhaseOutput> {
  
    public Map map;
    public TMP_Text gamePhaseText;
    public GameObject turnButtonContainer;
    public UIData uiData;
    public MapHighlighter mapHighlighter;
    public Transform cameraTransform;
    public RadarBlipLayer radarBlipLayer;

    public SFXLayer sfxLayer;

    public Transform alienAttackPrefab; 

    public static ProgressGamePhasePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ProgressGamePhaseOutput input) {
        if (uiData.gamePhase != input.currentPhase) {
            UpdateUI(input.currentPhase);
            UpdatePhaseText(input.currentPhase);
            uiData.gamePhase = input.currentPhase;
        }

        if (input.newAliens != null) {
            foreach (var newAlien in input.newAliens) {
                InstantiateAlien(newAlien);
            }
        }

        radarBlipLayer.ClearBlips();
        if (input.radarBlips != null) {
            foreach (var blipPosition in input.radarBlips) {
                var tile = map.GetTileAt(new Vector2(blipPosition.x, blipPosition.y));
                radarBlipLayer.AddBlip(tile.transform.position);
            }
        }

        if (input.alienActions != null) {
            StartCoroutine(AlienActionAnimation(input));
        }
    }

    IEnumerator AlienActionAnimation(ProgressGamePhaseOutput input) {
        foreach (var action in input.alienActions) {
            if (action.type == AlienActionType.Move) {
                var alien = GetAlienByIndex(action.index);
                var gridLocation = new Vector2(action.position.x, action.position.y);
                MoveAlien(
                    alien,
                    gridLocation,
                    ConvertDirection(action.facing)
                );
                if (!map.GetTileAt(gridLocation).foggy) {
                    FocusCameraOn(alien.transform);
                    yield return new WaitForSeconds(1);
                }
            } else {
                var alien = GetAlienByIndex(action.index);
                var tile = map.GetTileAt(new Vector2(action.position.x, action.position.y));
                var target = tile.GetActor<Soldier>();
                alien.Face(new Vector2(action.position.x, action.position.y));
                yield return new WaitForSeconds(0.5f);
                var attackSprite = sfxLayer.SpawnPrefab(alienAttackPrefab, Vector3.Lerp(alien.transform.position, target.transform.position, 0.5f), alien.transform.rotation);
                yield return new WaitForSeconds(0.5f);
                Destroy(attackSprite);
                if (action.attackResult == AttackResult.Killed) {
                    target.Destroy();
                    tile.RemoveActor();
                }
            }
        }
    }

    void UpdateUI(Data.GamePhase gamePhase) {
        turnButtonContainer.SetActive(false);
        uiData.ClearSelection();
        mapHighlighter.ClearHighlights();
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

    void FocusCameraOn(Transform target) {
        var cameraPosition = target.position;
        cameraPosition.z = cameraTransform.position.z;
        cameraTransform.position = cameraPosition;
    }
}
