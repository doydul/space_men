using UnityEngine;

public interface IWorld {

    void CreateExplosionMarker(Vector2 gridLocation, float opacity);

    void CreateRadarBlip(Vector2 gridLocation);

    void ClearRadarBlips();

    T GetActorAt<T>(Vector2 gridLocation);

    Tile GetTileAt(Vector2 gridLocation);

    void MakeBloodSplat(Actor actor);
}
