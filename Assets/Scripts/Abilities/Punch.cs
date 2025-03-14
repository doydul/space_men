using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "Punch", menuName = "Abilities/Punch", order = 1)]
public class Punch : Ability {
    
    public int minDamage = 1;
    public int maxDamage = 3;
    
    public AudioCollection audio;
    
    protected bool freeActionGiven;

    public override bool CanUse() {
        return owner.hasActions && possibleTargets.Length > 0;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    private Tile[] possibleTargets => Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.HasActor<Alien>()).ToArray();

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription($"{userFacingName}\nChoose Target");
        SideModal.instance.Show(description);
        yield return MapInputController.instance.SelectTileFrom(Color.red, possibleTargets);
        SideModal.instance.Hide();
        var hitTile = MapInputController.instance.selectedTile;
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        owner.actionsSpent += 1;
        owner.MoveAnimation();
        yield return GameplayOperations.PerformTurnAnimation(owner, Actor.FacingToDirection(hitTile.gridLocation - owner.gridLocation));
        yield return new WaitForSeconds(0.2f);
        owner.PlayAudio(audio.Sample());
        yield return new WaitForSeconds(0.2f);
        
        var damage = Random.Range(minDamage, maxDamage + 1);
        var alien = hitTile.GetActor<Alien>();
        alien.Hurt(damage, DamageType.IgnoreArmour);
        if (alien.dead && !freeActionGiven) {
            owner.actionsSpent -= 1;
            freeActionGiven = true;
        }
        
        yield return new WaitForSeconds(0.5f);
        owner.StationaryAnimation();
    }
    
    public override void Setup() {
        GameEvents.On(this, "player_turn_start", () => {
            freeActionGiven = false;
        });
    }
    
    public override void Teardown() {
        GameEvents.RemoveListener(this, "player_turn_start");
    }
}