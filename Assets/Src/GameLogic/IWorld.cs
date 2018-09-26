using UnityEngine;

public interface IWorld {

    void CreateExplosionMarker(Vector2 gridLocation, float opacity);

    T GetActorAt<T>(Vector2 gridLocation);

    Tile GetTileAt(Vector2 gridLocation);

    void MakeBloodSplat(Actor actor);
}
