using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "Punch", menuName = "Abilities/Punch", order = 1)]
public class Punch : Ability {
    
    public int minDamage = 1;
    public int maxDamage = 3;
    
    public AudioCollection audio;

    public override bool CanUse() {
        return owner.canAct && possibleTargets.Length > 0;
    }

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }
    
    private Tile[] possibleTargets => Map.instance.AdjacentTiles(owner.tile).Where(tile => tile.HasActor<Alien>()).ToArray();

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription("Punch\nChoose Target");
        yield return MapInputController.instance.SelectTileFrom(Color.red, possibleTargets);
        var hitTile = MapInputController.instance.selectedTile;
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        owner.actionsSpent += 1;
        owner.Face(hitTile.gridLocation);
        yield return new WaitForSeconds(0.2f);
        owner.PlayAudio(audio.Sample());
        yield return new WaitForSeconds(0.2f);
        
        var damage = Random.Range(minDamage, maxDamage + 1);
        hitTile.GetActor<Alien>().Hurt(damage, DamageType.Normal);
        
        yield return new WaitForSeconds(0.5f);
    }
}