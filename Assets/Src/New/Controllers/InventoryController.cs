using Interactors;
using Data;

public class InventoryController : Controller {

    public InventoryInitializer.Args args { private get; set; }

    public OpenWeaponSelectInteractor openWeaponSelectInteractor { get; set; }
    public OpenArmourSelectInteractor openArmourSelectInteractor { get; set; }

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
