using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollableList<T> {

    private List<T> collection;
    private int viewableItems;
    private int scrollIndex;

    public ScrollableList(List<T> collection, int viewableItems) {
        this.collection = collection;
        this.viewableItems = viewableItems;
    }

    public void IncreaseScroll() {
        if (scrollIndex < collection.Count - viewableItems) scrollIndex++;
    }

    public void DecreaseScroll() {
        if (scrollIndex > 0) scrollIndex--;
    }

    public List<T> GetCurrentView() {
        return collection.Skip(scrollIndex).Take(viewableItems).ToList();
    }

    public void Update(List<T> updatedCollection) {
        collection = updatedCollection;
    }
}
