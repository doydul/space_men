using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoldierSelectMenuController : SceneMenu {

    public const string sceneName = "SoldierSelect";

    public Text soldierItemInfo;
    public Text soldierStatInfo;
    public List<SoldierSelectPanelController> soldierPanels;

    private static int selectedSoldierIndex;

    private List<SoldierData> selectableSoldiers;
    private SoldierData replacementSoldier;

    public static void OpenMenu(int selectedSoldierIndex) {
        SoldierSelectMenuController.selectedSoldierIndex = selectedSoldierIndex;
        SceneManager.LoadScene(sceneName);
    }

    protected override void _Awake() {
        if (Squad.active == null) Squad.SetActive(Squad.GenerateDefault());

        selectableSoldiers = new List<SoldierData>(Squad.reserveSoldiers);
        selectableSoldiers.Add(Squad.activeSoldiers[selectedSoldierIndex]);

        for (int i = 0; i < selectableSoldiers.Count; i++) {
            soldierPanels[i].soldierData = selectableSoldiers[i];
        }
    }

    public void SelectReplacementSoldier(SoldierData replacementSoldier) {
        if (replacementSoldier == null) return;
        this.replacementSoldier = replacementSoldier;
        soldierItemInfo.text = "Armour: " + replacementSoldier.armour + "\n\nWeapon: " + replacementSoldier.weapon;
        soldierStatInfo.text = "Exp: " + replacementSoldier.exp;
    }

    public void FinaliseSelection() {
        Squad.ReplaceSoldier(selectedSoldierIndex, replacementSoldier);
        FadeToBlack(() => {
            ArmouryMenuController.OpenMenu();
        });
    }

    public void ScrollSoldiersLeft() {

    }

    public void ScrollSoldiersRight() {

    }
}
