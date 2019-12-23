using Interactors;
using Data;

public class LoadGameController : Controller {

    public LoadGameInteractor loadGameInteractor { get; set; }
    public OpenLoadingMenuInteractor openLoadingMenuInteractor { get; set; }

    public void InitializeLoadGamePage() {
        if (!disabled) {
            openLoadingMenuInteractor.Interact(new OpenLoadingMenuInput {});
        }
    }

    public void LoadGame(int slot) {
        if (!disabled) {
            loadGameInteractor.Interact(new LoadGameInput {
                slotId = slot
            });
        }
    }
}
