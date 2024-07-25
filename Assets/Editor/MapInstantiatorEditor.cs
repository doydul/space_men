using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor (typeof(MapInstantiator))]
public class MapInstantiatorEditor : Editor {

    private MapInstantiator mapGen => target as MapInstantiator;
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button(" Generate ")) mapGen.Generate(new MapInstantiator.Blueprint {
            vents = 5,
            loots = 3,
            corridors = 6,
            secondaryCorridors = 4,
            rooms = 5
        });
    }
}