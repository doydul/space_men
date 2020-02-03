using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;

public class InventoryInitializer : InitializerBase {

    static Args args { get; set; }

    public static void OpenScene(Args arguments) {
        args = arguments;
        SceneManager.LoadScene("InventoryView");
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(InventoryController), new Dictionary<Type, Type> {
            { typeof(OpenInventoryInteractor), typeof(OpenInventoryPresenter) },
            { typeof(OpenWeaponSelectInteractor), typeof(OpenWeaponSelectPresenter) },
            { typeof(OpenArmourSelectInteractor), typeof(OpenArmourSelectPresenter) }
        });
    }

    protected override void GenerateDependencies() {}

    protected override void Initialize() {
        FindObjectOfType<InventoryController>().InitPage(args);
    }

    public struct Args {
        public long metaSoldierId;
    }
}
