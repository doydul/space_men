using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SquadSelectMenuController : SceneMenu {

    public const string sceneName = "SquadSelect";

    public List<SquadPanelController> squadPanels;

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        DataPersistence.Load();

        for (int i = 0; i < DataPersistence.squads.Count; i++) {
            squadPanels[i].squad = DataPersistence.squads[i];
        }
    }

    public void Next(Squad squad) {
        if (squad != null) {
            Squad.SetActive(squad);
        } else {
            Squad.SetActive(Squad.GenerateDefault());
        }

        FadeToBlack(() => {
            MissionOverviewController.OpenMenu();
        });
    }

    public void Back() {
        FadeToBlack(() => {
            MainMenuController.OpenMenu();
        });
    }
}
