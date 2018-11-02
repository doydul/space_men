using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoldierViewController : SceneMenu {

    public const string sceneName = "TemplarView";

    public static SoldierData activeSoldier;

    public Text infoText;

    public static void OpenMenu(SoldierData activeSoldier) {
        SoldierViewController.activeSoldier = activeSoldier;
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        infoText.text = "Weapon: " + activeSoldier.weapon + "\nArmour: " + activeSoldier.armour;
    }

    public void Back() {
        FadeToBlack(() => {
            ArmouryMenuController.OpenMenu();
        });
    }

    public void ClickWeapon() {
        FadeToBlack(() => {
            SelectionMenuController.OpenMenu(activeSoldier, editWeapon: true);
        });
    }

    public void clickArmour() {
        FadeToBlack(() => {
            SelectionMenuController.OpenMenu(activeSoldier, editWeapon: false);
        });
    }
}
