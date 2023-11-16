using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Overwatch", menuName = "Abilities/Overwatch", order = 1)]
public class Overwatch : ReactionAbility {
    
    public override bool CanUse() {
        return owner.hasAmmo && owner.canAct;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    public IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Overwatch\nChoose Facing");
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.open).ToArray());
        if (MapInputController.instance.selectedTile == null) yield break;
        AbilityInfoPanel.instance.Hide();

        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        owner.Face(MapInputController.instance.selectedTile.gridLocation);
        owner.reaction = this;
    }

    public override bool TriggersReaction(Tile tile, Actor actor) {
        return owner.InRange(tile.gridLocation) && owner.WithinSightArc(tile.gridLocation) && owner.CanSee(tile.gridLocation) && actor is Alien;
    }

    public override IEnumerator PerformReaction(Tile tile) {
        owner.shotsSpent += 1;
        owner.reaction = null;
        yield return owner.PerformShoot(tile.GetActor<Alien>());
    }
}