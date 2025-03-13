using UnityEngine;
using UnityEditor;
 
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
        for (int i = 0; i < levelProgIterations; i++) {
            Debug.Log($"------------ LEVEL {i + 2} --------------------------------------------------------------------------");
            Campaign.NextLevel(save);
        }
    }
}