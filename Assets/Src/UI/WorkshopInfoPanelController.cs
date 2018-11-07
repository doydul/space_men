using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class WorkshopInfoPanelController : MonoBehaviour {

    public GameObject leftButton;
    public Text leftButtonText;
    public GameObject rightButton;
    public Text rightButtonText;
    public Text infoText;
    public Image infoImage;
    public UnityEvent updateInterface;

    private InfoPanelState state;

    void Awake() {
        infoImage.enabled = false;
    }

    public void PressLeftButton() {
        state.PressLeftButton();
    }

    public void PressRightButton() {
        state.PressRightButton();
    }

    public void SelectBlueprint(InventoryItem blueprint) {
        state = new BlueprintSelectedState(blueprint);
        state.Setup(this);
        SetInfoTextAndImage(blueprint);
    }

    public void SelectInventoryItem(InventoryItem item) {
        state = new InventoryItemSelectedState(item);
        state.Setup(this);
        SetInfoTextAndImage(item);
    }

    protected void SetInfoTextAndImage(InventoryItem item) {
        infoImage.enabled = item != null;
        infoText.text = new ItemDescriptionGenerator(item).Generate();
        infoImage.sprite = new SpriteSelector(item).Select();
    }

    private interface InfoPanelState {

        void Setup(WorkshopInfoPanelController context);
        void PressLeftButton();
        void PressRightButton();
    }

    private class BlueprintSelectedState : InfoPanelState {

        private InventoryItem item;
        private WorkshopInfoPanelController context;

        public BlueprintSelectedState(InventoryItem item) {
            this.item = item;
        }

        public void Setup(WorkshopInfoPanelController context) {
            context.leftButton.SetActive(false);
            context.rightButtonText.text = "Fabricate";
            this.context = context;
        }

        public void PressLeftButton() {}

        public void PressRightButton() {
            new Workshop(item).FabricateItem();
            context.updateInterface.Invoke();
        }
    }

    private class InventoryItemSelectedState : InfoPanelState {

        private InventoryItem item;
        private WorkshopInfoPanelController context;

        public InventoryItemSelectedState(InventoryItem item) {
            this.item = item;
        }

        public void Setup(WorkshopInfoPanelController context) {
            context.leftButton.SetActive(true);
            context.leftButtonText.text = "Scrap";
            context.rightButtonText.text = "Analyse";
            this.context = context;
        }

        public void PressLeftButton() {
            new Workshop(item).ScrapItem();
            context.updateInterface.Invoke();
            context.SelectInventoryItem(null);
        }

        public void PressRightButton() {
            new Workshop(item).AnalyseItem();
            context.updateInterface.Invoke();
            context.SelectInventoryItem(null);
        }
    }
}
