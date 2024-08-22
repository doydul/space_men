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
        // AnimationManager.instance.StartAnimation(PerformUse());
        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        owner.reaction = this;
        owner.ShowAbilityIcon("Overwatch");
    }

    public IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Overwatch\nChoose Facing");
        yield return MapInputController.instance.SelectTileFrom(Color.red, Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.open).ToArray());
        if (MapInputController.instance.selectedTile == null) yield break;
        AbilityInfoPanel.instance.Hide();

        owner.Face(MapInputController.instance.selectedTile.gridLocation);
        owner.actionsSpent += 100;
        owner.tilesMoved += 100;
        owner.reaction = this;
        owner.ShowAbilityIcon("Overwatch");
    }

    public override bool TriggersReaction(Tile tile, Actor actor) {
        return !tile.foggy && owner.InRange(tile.gridLocation) && owner.CanSee(tile.gridLocation) && actor is Alien;
    }

    public override IEnumerator PerformReaction(Tile tile) {
        owner.HideAbilityIcon();
        owner.shotsSpent += 1;
        owner.reaction = null;
        yield return owner.PerformShoot(tile.GetActor<Alien>());
    }
}