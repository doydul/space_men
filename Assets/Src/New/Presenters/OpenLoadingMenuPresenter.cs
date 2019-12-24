using Data;

public class OpenLoadingMenuPresenter : Presenter, IPresenter<OpenLoadingMenuOutput> {
  
    public static OpenLoadingMenuPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public LoadGameController loadGameController;
    public SavePageSlot[] slots;
    
    public void Present(OpenLoadingMenuOutput input) {
        foreach (var slot in input.slots) {
            if (slot.containsSaveData) {
                slots[slot.slotId].SetText("Continue");
            }
        }
    }
}

