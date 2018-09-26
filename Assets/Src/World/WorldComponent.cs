using UnityEngine;

public class WorldComponent : MonoBehaviour, IWorld {

    public Map map;
    public MapMarkerController mapMarkerController;
    public BloodSplatController bloodSplatController;

    public void CreateExplosionMarker(Vector2 gridLocation, float opacity) {
        mapMarkerController.CreateExplosionMarker(gridLocation, opacity);
    }

    public T GetActorAt<T>(Vector2 gridLocation) {
        return map.GetActorAt<T>(gridLocation);
    }

    public Tile GetTileAt(Vector2 gridLocation) {
        return map.GetTileAt(gridLocation);
    }

    public void MakeBloodSplat(Actor actor) {
        bloodSplatController.MakeSplat(actor);
    }
}
