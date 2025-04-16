using System.Collections.Generic;
using System.Linq;

public class Loot {
    public int credits;
    public List<InventoryItem> items = new();

    public bool hasItem => items != null && items.Count > 0;
    public bool hasCredits => credits > 0;
    
    public Loot(params InventoryItem[] items) {
        this.items = items.ToList();
    }
    public Loot(int credits) {
        this.credits = credits;
    }
    public Loot(int credits, params InventoryItem[] items) {
        this.credits = credits;
        this.items = items.ToList();
    }
    
    public Loot AddItem(InventoryItem item) {
        items.Add(item);
        return this;
    }
}