using System.Collections.Generic;
using Data;

namespace Interactors {
    
    public class OpenLoadingMenuInteractor : Interactor<OpenLoadingMenuOutput> {

        public IMetaGameStateStore metaGameStateStore { private get; set; }

        public void Interact(OpenLoadingMenuInput input) {
            var output = new OpenLoadingMenuOutput();
            
            var slots = new List<OpenLoadingMenuOutput.Slot>();
            for (int i = 0; i < 4; i++) {
                var slot = new OpenLoadingMenuOutput.Slot { slotId = i };
                if (metaGameStateStore.SaveExists(i)) slot.containsSaveData = true;
                slots.Add(slot);
            }
            output.slots = slots.ToArray();
            
            presenter.Present(output);
        }
    }
}
