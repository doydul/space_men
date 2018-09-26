using UnityEngine;

public class ExplodableWrapper : IIterableGrid {

    private IWorld world;

    public ExplodableWrapper(IWorld world) {
        this.world = world;
    }

    public bool ShouldIterate(Vector2 gridLocation) {
        return world.GetTileAt(gridLocation).open;
    }
}
