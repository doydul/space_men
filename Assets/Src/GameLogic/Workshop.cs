using UnityEngine;

public class Workshop {

    private InventoryItem item;

    public bool canAnalyse { get { return Squad.credits >= ItemValue(); } }
    public bool canFabricate { get { return Squad.credits >= ItemValue(); } }

    public Workshop(InventoryItem item) {
        this.item = item;
    }

    public void ScrapItem() {
        Squad.ChangeCredits(ItemValue());
        Squad.items.Remove(item);
    }

    public void AnalyseItem() {
        if (!canAnalyse) return;
        Squad.ChangeCredits(-ItemValue());
        Squad.items.Remove(item);
        Squad.blueprints.Add(new ItemBlueprint() { item = item });
    }

    public void FabricateItem() {
        if (!canFabricate) return;
        Squad.ChangeCredits(-ItemValue());
        Squad.items.Add(item.Dup());
    }

    // Private

    private int ItemValue() {
        // if (item.isWeapon) {
            return 0;
        // } else {
        //     return 0;
        // }
    }
}
