using Interactors;
using Data;

public class InventoryController : Controller {

    public InventoryInitializer.Args args { private get; set; }

    public OpenInventoryInteractor openInventoryInteractor { private get; set; }
    public OpenWeaponSelectInteractor openWeaponSelectInteractor { private get; set; }
    public OpenArmourSelectInteractor openArmourSelectInteractor { private get; set; }

    public void InitPage(InventoryInitializer.Args args) {
        this.args = args;
        openInventoryInteractor.Interact(new OpenInventoryInput {
            metaSoldierId = args.metaSoldierId
        });
    }

    public void GoToWeaponSelect() {
        if (!disabled) {
            openWeaponSelectInteractor.Interact(new OpenWeaponSelectInput {
                metaSoldierId = args.metaSoldierId
            });
        }
    }

    public void GoToArmourSelect() {
        if (!disabled) {
            openArmourSelectInteractor.Interact(new OpenArmourSelectInput {
                metaSoldierId = args.metaSoldierId
            });
        }
    }
}
