using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;
using Data;

public class ArmouryInitializer : InitializerBase {

    static ArmouryMenuArgs args { get; set; }

    public static void OpenScene(ArmouryMenuArgs arguments) {
        SceneManager.LoadScene("Armoury");
        args = arguments;
    }

    protected override void GenerateControllerMapping() {
    }

    protected override void GenerateDependencies() {
        // dependencies.Add(new SomeDep());
    }

    protected override void Initialize() {
        FindObjectOfType<ArmouryMenu>().Init(args);
    }
}

public struct ArmouryMenuArgs {

    public SoldierDisplayInfo[] soldierInfo;
}
