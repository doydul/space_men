using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MapBlueprint", menuName = "MapBlueprint", order = 10)]
public class MapBlueprint : ScriptableObject {
    public int vents;
    public int loots;
    public int equipments;
    public int corridors;
    public int secondaryCorridors;
    public int rooms;
}