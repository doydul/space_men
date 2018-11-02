using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : SceneMenu {

    public const string sceneName = "MainMenu";

    public static void OpenMenu() {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame() {
        FadeToBlack(() => {
            SquadSelectMenuController.OpenMenu();
        });
    }
}
