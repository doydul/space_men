using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "Scan", menuName = "Abilities/Scan", order = 1)]
public class Scan : CooldownAbility {
    
    public int range;
    public int scanRadius;

    public override void Use() {
        AnimationManager.instance.StartAnimation(PerformUse());
    }

    private IEnumerator PerformUse() {
        AbilityInfoPanel.instance.ShowDescription($"{userFacingName}\nChoose Target");
        SideModal.instance.Show(description);
        yield return MapInputController.instance.SelectTileFrom(Color.yellow, Map.instance.iterator.RadiallyFrom(owner.gridLocation, range)
          .Where(tile => tile.open).ToArray());
        var hitTile = MapInputController.instance.selectedTile;
        SideModal.instance.Hide();
        if (hitTile == null) yield break;
        AbilityInfoPanel.instance.Hide();
        
        SetCooldown();
        foreach (var tile in Map.instance.iterator.RadiallyFrom(hitTile.gridLocation, scanRadius)) tile.RemoveFoggy();
    }
}