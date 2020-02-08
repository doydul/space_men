using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;

using Data;

public class ProgressGamePhasePresenter : Presenter, IPresenter<ProgressGamePhaseOutput> {
    
    public MapController mapInput;
    public UIController uiInput;
    public Map map;
    public TMP_Text gamePhaseText;
    public TMP_Text threatCounterText;
    public GameObject turnButtonContainer;
    public UIData uiData;
    public MapHighlighter mapHighlighter;
    public Transform cameraTransform;
    public RadarBlipLayer radarBlipLayer;
    public GameObject missions;

    public SFXLayer sfxLayer;

    public Transform alienAttackPrefab;
    public Transform healthBarPrefab;
    public Transform deflectMarkerPrefab;

    public static ProgressGamePhasePresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ProgressGamePhaseOutput input) {
        mapInput.Disable();
        uiInput.Disable();

        threatCounterText.text = "turns until threat increase: " + input.threatCountdown;
        if (input.currentThreatLevel != uiData.threatLevel) {
            uiData.threatLevel = input.currentThreatLevel;
            missions.SendMessage("OnThreatEscalation", input.currentThreatLevel);
        }

        if (uiData.gamePhase != input.currentPhase) {
            UpdateUI(input.currentPhase);
            UpdatePhaseText(input.currentPhase);
            UpdateSoldierIndicators(input.currentPhase, input.shootingStats);
            uiData.gamePhase = input.currentPhase;
            missions.SendMessage("OnPhaseChange");
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
        } else {
            mapInput.Enable();
            uiInput.Enable();
        }
    }

    IEnumerator AlienActionAnimation(ProgressGamePhaseOutput input) {
        foreach (var action in input.alienActions) {
            if (action.type == AlienActionType.Move) {
                var alien = GetAlienByIndex(action.index);
                yield return MoveAlienWithAnimation(alien, new Vector2(action.position.x, action.position.y), ConvertDirection(action.facing));
            } else {
                var alien = GetAlienByIndex(action.index);
                var tile = map.GetTileAt(new Vector2(action.position.x, action.position.y));
                var target = tile.GetActor<Soldier>();
                target.health = action.damageInstance.victimHealthAfterDamage;
                yield return AlienAttackAnimation(alien, target);
                if (action.damageInstance.attackResult == AttackResult.Hit) {
                    var healthBarGO = sfxLayer.SpawnPrefab(healthBarPrefab, target.transform.position);
                    var healthBar = healthBarGO.GetComponent<HealthBar>();
                    healthBar.SetPercentage(target.healthPercentage);
                    yield return new WaitForSeconds(1);
                    Destroy(healthBarGO);
                } else if (action.damageInstance.attackResult == AttackResult.Hit) {
                    yield return MarkerAnimationFor(action.damageInstance);
                } else if (action.damageInstance.attackResult == AttackResult.Killed) {
                    target.Destroy();
                    tile.RemoveActor();
                }
            }
        }
        
        mapInput.Enable();
        uiInput.Enable();
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

    void UpdateSoldierIndicators(Data.GamePhase gamePhase, ShootingStats[] shootingStats) {
        if (gamePhase == Data.GamePhase.Shooting) {
            foreach (var stats in shootingStats) {
                var soldier = map.GetActorByIndex(stats.soldierID) as Soldier;
                var soldierUI = soldier.GetComponent<SoldierUIController>();
                soldierUI.HideAll();
                soldierUI.SetAmmoCount(stats.shots);
            }
        } else {
            foreach (var soldier in map.GetActors<Soldier>()) {
                var soldierUI = soldier.GetComponent<SoldierUIController>();
                soldierUI.HideAll();
            }
        }
    }

    Transform InstantiateAlien(Data.Alien newAlien) {
        var alienTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;
        var alien = alienTransform.GetComponent<Alien>() as Alien;
        alien.FromData(Resources.Load<AlienData>("Aliens/" + newAlien.alienType));
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

    IEnumerator AlienAttackAnimation(Alien alien, Actor target) {
        alien.Face(target.gridLocation);
        yield return new WaitForSeconds(0.5f);
        var attackSprite = sfxLayer.SpawnPrefab(alienAttackPrefab, Vector3.Lerp(alien.realLocation, target.realLocation, 0.5f), alien.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(attackSprite);
    }

    IEnumerator MoveAlienWithAnimation(Alien alien, Vector2 gridLocation, Actor.Direction newFacing) {
        MoveAlien(
            alien,
            gridLocation,
            newFacing
        );
        if (!map.GetTileAt(gridLocation).foggy) {
            FocusCameraOn(alien.transform);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator MarkerAnimationFor(DamageInstance damageInstance) {
        if (damageInstance.attackResult != AttackResult.Deflected) yield break;
        var actor = map.GetActorByIndex(damageInstance.victimIndex);
        var marker =  sfxLayer.SpawnPrefab(deflectMarkerPrefab, actor.realLocation);
        yield return new WaitForSeconds(1);
        Destroy(marker);
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
