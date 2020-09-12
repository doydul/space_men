using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;
using Data;

public class ArmouryInitializer : InitializerBase {

    public SoldierNameGenerator nameGenerator;

    public static void OpenScene() {
        SceneManager.LoadScene("Armoury");
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(ArmouryController), new Dictionary<Type, Type> {
            { typeof(OpenArmouryInteractor), typeof(OpenArmouryPresenter) },
            { typeof(OpenSoldierSelectInteractor), typeof(OpenSoldierSelectPresenter) },
            { typeof(HireSolderInteractor), typeof(HireSolderPresenter) }
        });
    }

    protected override void GenerateDependencies() {
        if (MetaGameState.instance == null) {
            MetaGameState.Load(0, DebugSaveGenerator.Generate());
        }

        dependencies.Add(new SoldierNameGeneratorWrapper(nameGenerator));
    }

    protected override void Initialize() {
        FindObjectOfType<ArmouryController>().InitPage();
    }
}
