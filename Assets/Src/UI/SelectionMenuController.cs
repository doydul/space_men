using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenuController : SceneMenu {

    public const string sceneName = "SelectionMenu";

    public Text infoTextLeft;
    public Text infoTextRight;
    public Image mainPicture;
    public List<SelectionPanelController> selectionPanels;

    private static SelectionMode mode;

    public static void OpenMenu(int selectedSoldierIndex) {
        mode = new SoldierSelectMode(selectedSoldierIndex);
        SceneManager.LoadScene(sceneName);
    }

    public static void OpenMenu(SoldierData selectedSoldier, bool editWeapon) {
        if (editWeapon) {
            mode = new WeaponSelectMode(selectedSoldier);
        } else {
            mode = new ArmourSelectMode(selectedSoldier);
        }
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        if (mode == null) {
            Squad.SetActive(Squad.GenerateDefault());
            mode = new WeaponSelectMode(Squad.activeSoldiers[0]);
        }

        for (int i = 0; i < mode.selectableItems.Count; i++) {
            selectionPanels[i].selectableItem = mode.selectableItems[i];
        }
    }

    public void SelectItem(SelectableItem selectableItem) {
        if (selectableItem == null) return;

        mainPicture.sprite = selectableItem.sprite;
        mainPicture.gameObject.SetActive(true);

        mode.Select(selectableItem);
        infoTextLeft.text = selectableItem.leftText;
        infoTextRight.text = selectableItem.rightText;
    }

    public void FinaliseSelection() {
        FadeToBlack(() => {
            mode.Finalise();
        });
    }

    public void ScrollLeft() {

    }

    public void ScrollRight() {

    }
}
