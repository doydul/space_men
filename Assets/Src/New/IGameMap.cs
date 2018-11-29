using UnityEngine;
using System.Collections.Generic;

public interface IGameMap {

    List<Soldier> soldiers { get; }

    List<Tile> tiles { get; }
}
