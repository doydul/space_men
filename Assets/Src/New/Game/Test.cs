using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public Map map;

    void Start() {
        MakeSoldier();
        MakeAlien(new Vector2(8, 8));
        MakeAlien(new Vector2(8, 7));
        MakeAlien(new Vector2(8, 6));
        MakeAlien(new Vector2(8, 5));
    }

    void MakeSoldier() {
        var soldierData = SoldierData.GenerateDefault();
        var gridLocation = new Vector2(1, 1);
        var trans = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Soldier")) as Transform;

        var soldier = trans.GetComponent<Soldier>();
        soldier.FromData(soldierData);

        map.GetTileAt(gridLocation).SetActor(trans);
    }

    void MakeAlien(Vector2 at) {
        var alienTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/Alien")) as Transform;
        var alien = alienTransform.GetComponent<Alien>() as Alien;
        alien.FromData(Resources.Load<AlienData>("Aliens/Alien"));
        var spriteTransform = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/AlienSprites/AlienAlienSprite")) as Transform;
        spriteTransform.parent = alienTransform;
        spriteTransform.localPosition = Vector3.zero;
        alien.image = spriteTransform;

        map.GetTileAt(at).SetActor(alienTransform);
    }

    public void TestPathing() {
        var aliensCopy = map.GetActors<Alien>();
        var wrapper = new AlienPathingMapWrapper(map, aliensCopy);
        while (aliensCopy.Count > 0) {
            int unmoved = aliensCopy.Count;
            foreach (var alien in new List<Alien>(aliensCopy)) {
                var output = new AlienPathFinder2(wrapper, new BasicAlienPathingWrapper(map)).BestMoveLocation(alien.gridLocation, alien.movement);
                var tile = map.GetTileAt(output.targetLocation);
                if (alien.tile == tile) {
                    aliensCopy.Remove(alien);
                } else if (tile.actor == null) {
                    aliensCopy.Remove(alien);
                    alien.MoveTo(tile);
                    alien.TurnTo(output.facing);
                }
            }
            if (unmoved == aliensCopy.Count) break;
        }
    }
}
