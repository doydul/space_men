using UnityEngine;

public class Workshop {

    private InventoryItem item;

    public bool canAnalyse { get { return Squad.credits >= item.value; } }
    public bool canFabricate { get { return Squad.credits >= item.value; } }

    public Workshop(InventoryItem item) {
        this.item = item;
    }

    public void ScrapItem() {
        Squad.ChangeCredits(item.value);
        Squad.items.Remove(item);
    }

    public void AnalyseItem() {
        Squad.ChangeCredits(-item.value);
        Squad.items.Remove(item);
        Squad.blueprints.Add(new ItemBlueprint() { item = item });
    }

    public void FabricateItem() {
        Squad.ChangeCredits(-item.value);
        Squad.items.Add(item.Dup());
    }
}
