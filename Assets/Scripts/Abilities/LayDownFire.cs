using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "LayDownFire", menuName = "Abilities/LayDownFire", order = 1)]
public class LayDownFire : ReactionAbility {
    
    private int shotsRemaining;

    public override bool CanUse() {
        return owner.hasAmmo && owner.canAct;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
        owner.ShowAbilityIcon("LayDownFire");
    }

    public IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Lay Down Fire\nChoose Facing");
        SideModal.instance.Show(description);
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.open).ToArray());
        SideModal.instance.Hide();
        if (MapInputController.instance.selectedTile == null) yield break;
        AbilityInfoPanel.instance.Hide();

        shotsRemaining = owner.shots;
        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        owner.Face(MapInputController.instance.selectedTile.gridLocation);
        owner.reaction = this;
        yield return PerformShots();
    }

    public override bool TriggersReaction(Tile tile, Actor actor) {
        return shotsRemaining > 0 && !tile.foggy && owner.InRange(tile.gridLocation) && owner.WithinSightArc(tile.gridLocation) && owner.CanSee(tile.gridLocation) && actor is Alien;
    }

    public override IEnumerator PerformReaction(Tile tile) {
        yield return PerformShots();
    }

    private IEnumerator PerformShots() {
        var aliensInSight = Map.instance.GetActors<Alien>().Where(alien => !alien.tile.foggy && owner.CanSee(alien.gridLocation) && owner.InRange(alien.gridLocation) && owner.WithinSightArc(alien.gridLocation)).ToList();
        while (shotsRemaining > 0 && aliensInSight.Count > 0) {
            if (shotsRemaining >= owner.shots) owner.shotsSpent += 1;
            var randAlien = aliensInSight[Random.Range(0, aliensInSight.Count())];
            shotsRemaining -= 1;
            yield return GameplayOperations.PerformSoldierSingleShot(owner, randAlien);
            if (randAlien.dead) aliensInSight.Remove(randAlien);
        }
        if (shotsRemaining <= 0) owner.HideAbilityIcon();
    }
}