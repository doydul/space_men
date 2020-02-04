using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;
using Data;

public class ArmouryInitializer : InitializerBase {

    public static void OpenScene() {
        SceneManager.LoadScene("Armoury");
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(ArmouryController), new Dictionary<Type, Type> {
            { typeof(OpenArmouryInteractor), typeof(OpenArmouryPresenter) },
            { typeof(OpenSoldierSelectInteractor), typeof(OpenSoldierSelectPresenter) }
        });
    }

    protected override void GenerateDependencies() {
        // dependencies.Add(new SomeDep());
    }

    protected override void Initialize() {
        FindObjectOfType<ArmouryController>().InitPage();
    }
}
