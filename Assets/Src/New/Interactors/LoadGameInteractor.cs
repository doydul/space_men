using Data;
using Workers;

namespace Interactors {
    
    public class LoadGameInteractor : Interactor<LoadGameOutput> {

        public IMetaGameStateStore metaGameStateStore { private get; set; }

        public void Interact(LoadGameInput input) {
            var output = new LoadGameOutput();
            
            var save = metaGameStateStore.GetSave(input.slotId);
            MetaGameState.Load(input.slotId, save);
            output.success = true;
            
            presenter.Present(output);
        }
    }
}
