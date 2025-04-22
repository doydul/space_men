using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "LayDownFire", menuName = "Abilities/LayDownFire", order = 1)]
public class LayDownFire : ReactionAbility {
    
    int shotsRemaining;
    bool spunUp;
    
    public override IEnumerable<AbilityCondition> Conditions() {
        yield return new HasAmmo();
        yield return new HasAction();
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    public override void Setup() {
        GameEvents.On(this, "player_turn_start", PlayerTurnStart);
    }
    
    public override void Teardown() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }
    
    public IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription($"{userFacingName}\nChoose Facing");
        SideModal.instance.ShowCollapsible(description);
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.open).ToArray());
        SideModal.instance.Hide();
        if (MapInputController.instance.selectedTile == null) yield break;
        AbilityInfoPanel.instance.Hide();

        shotsRemaining = owner.shots;
        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        yield return GameplayOperations.PerformTurnAnimation(owner, Actor.FacingToDirection(MapInputController.instance.selectedTile.gridLocation - owner.gridLocation));
        owner.reaction = this;
        owner.ShowAbilityIcon(this);
        yield return PerformShots();
    }

    public override bool TriggersReaction(Tile tile, Actor actor) {
        return shotsRemaining > 0 && !tile.foggy && owner.InRange(tile.gridLocation) && owner.WithinSightArc(tile.gridLocation) && owner.CanSee(tile.gridLocation) && actor is Alien;
    }

    public override IEnumerator PerformReaction(Tile tile) {
        yield return PerformShots();
    }

    IEnumerator PerformShots() {
        var aliensInSight = Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation) && owner.WithinSightArc(alien.gridLocation)).ToList();
        bool woundUp = false;
        if (shotsRemaining > 0 && aliensInSight.Count > 0) {
            woundUp = true;
            owner.AimAnimation();
            yield return new WaitForSeconds(0.3f);
            if (!spunUp) {
                spunUp = true;
                yield return owner.PerformPlayAudio(owner.weapon.audio.spinUp);
            }
            owner.PlayAudioRepeat(owner.weapon.audio.spinning);
        }
        while (shotsRemaining > 0 && aliensInSight.Count > 0) {
            if (shotsRemaining >= owner.shots) owner.shotsSpent += 1;
            var randAlien = aliensInSight[Random.Range(0, aliensInSight.Count())];
            shotsRemaining -= 1;
            yield return GameplayOperations.PerformSoldierSingleShot(owner, randAlien);
            aliensInSight = aliensInSight.Where(alien => !alien.dead).ToList();
        }
        if (shotsRemaining <= 0) owner.HideAbilityIcon();
        if (woundUp) {
            owner.StopRepeatingAudio();
            owner.PlayAudio(owner.weapon.audio.spinDown);
            yield return new WaitForSeconds(0.3f);
        }
        owner.IdleAnimation();
    }
    
    void PlayerTurnStart() {
        spunUp = false;
    }
}