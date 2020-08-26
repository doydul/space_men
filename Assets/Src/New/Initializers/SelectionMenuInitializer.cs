using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Interactors;
using Workers;

public class SelectionMenuInitializer : InitializerBase {

    static Args args { get; set; }

    public SelectionMenu selectionMenu;

    public static void OpenScene(Args arguments) {
        SceneManager.LoadScene("SelectionMenu");
        args = arguments;
    }

    protected override void GenerateControllerMapping() {
        controllerMapping.Add(typeof(SelectionMenuController), new Dictionary<Type, Type> {
           { typeof(EquipItemInteractor), typeof(EquipItemPresenter) },
           { typeof(AddSoldierToSquadInteractor), typeof(AddSoldierToSquadPresenter) },
           { typeof(SelectionMenuCancelInteractor), typeof(SelectionMenuCancelPresenter) }
        });
    }

    protected override void GenerateDependencies() {
        dependencies.Add(args);
    }

    protected override void Initialize() {
        selectionMenu.Init(args);
    }

    public struct Args {
        public Action backAction;
        public Selectable[] selectables;
        public Selectable currentSelection;

        public struct Selectable {
            public int index;
            public long newOwnerId;
            public SelectableType type;
            public long id;
            public string name;
        }

        public enum SelectableType {
            Soldiers,
            Weapons,
            Armour
        }
    }
}
