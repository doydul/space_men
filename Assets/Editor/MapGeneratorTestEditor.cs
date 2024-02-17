using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor (typeof(MapGeneratorTest))]
public class MapGeneratorTestEditor : Editor {

    private MapGeneratorTest mapGen => target as MapGeneratorTest;
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button(" Generate ")) mapGen.Generate();
    }
}