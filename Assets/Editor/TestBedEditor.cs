using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor (typeof(TestBed))]
public class TestBedEditor : Editor {

    private TestBed testBed => target as TestBed;
    
    int levelProgIterations;
    
    public override void OnInspectorGUI() {
        levelProgIterations = int.Parse(GUILayout.TextField(levelProgIterations.ToString(), 4));
        if (GUILayout.Button(" Lvl Prog Test ")) TestLevelProg();
        GUILayout.Space(50);
        base.OnInspectorGUI();
    }
    
    void TestLevelProg() {
        var save = PlayerSave.New();
        save.IncreaseDifficulty(0.2f);
        Mission.Generate(save);
        for (int i = 0; i < levelProgIterations; i++) {
            Debug.Log($"------------ LEVEL {i + 2} --------------------------------------------------------------------------");
            Campaign.NextLevel(save);
        }
    }
    
    void TestSpawns(float avrgSpawns) {
        var results = new List<int>();
        for (int i = 0; i < 10000; i++) {
            var stdDev = avrgSpawns * 0.4f;
            if (avrgSpawns < 0.5f) {
                float t = avrgSpawns / 0.5f;
                stdDev = t * stdDev + (1 - t) * avrgSpawns * 3;
            }
            int spawns = Mathf.Max(0, (int)Mathf.Round(GaussianNumber.Generate(avrgSpawns, stdDev)));
            results.Add(spawns);
        }
        Debug.Log($"Desired: {avrgSpawns}, Actual: {results.Sum() / 10000f}");
    }
}