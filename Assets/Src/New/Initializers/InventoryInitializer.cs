using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;

public class InventoryInitializer : InitializerBase {

    static Args args { get; set; }

    public static void OpenScene(Args arguments) {
        SceneManager.LoadScene("TemplarView");
        args = arguments;
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(InventoryController), new Dictionary<Type, Type> {
            { typeof(OpenWeaponSelectInteractor), typeof(OpenWeaponSelectPresenter) },
            { typeof(OpenArmourSelectInteractor), typeof(OpenArmourSelectPresenter) }
        });
    }

    protected override void GenerateDependencies() {}

    protected override void Initialize() {
        FindObjectOfType<InventoryController>().args = args;
    }

    public struct Args {
        public long metaSoldierId;
        public string armourName;
        public string weaponName;
    }
}
