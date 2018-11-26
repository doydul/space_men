using System;

public class UIInputHandler : PlayerActionHandler.IPlayerActionInput {

    Action turnSoldierLeftListener;
    Action turnSoldierRightListener;
    Action turnSoldierUpListener;
    Action turnSoldierDownListener;
    Action continueButtonListener;
    Action<Tile> interactWithTileListener;

    public override void SetTurnSoldierLeftListener(Action listener) {
        turnSoldierLeftListener = listener;
    }

    public override void SetTurnSoldierRightListener(Action listener) {
        turnSoldierRightListener = listener;
    }

    public override void SetTurnSoldierUpListener(Action listener) {
        turnSoldierUpListener = listener;
    }

    public override void SetTurnSoldierDownListener(Action listener) {
        turnSoldierDownListener = listener;
    }

    public override void SetContinueButtonListener(Action listener) {
        continueButtonListener = listener;
    }

    public override void SetInteractWithTileListener(Action<Tile> listener) {
        interactWithTileListener = listener;
    }

    public void TriggerTurnSoldierLeft() {
        turnSoldierLeftListener();
    }

    public void TriggerTurnSoldierRight() {
        turnSoldierRightListener();
    }

    public void TriggerTurnSoldierUp() {
        turnSoldierUpListener();
    }

    public void TriggerTurnSoldierDown() {
        turnSoldierDownListener();
    }

    public void TriggerContinueButton() {
        continueButtonListener();
    }

    public void TriggerInteractWithTile(Tile tile) {
        interactWithTileListener(tile);
    }
}
