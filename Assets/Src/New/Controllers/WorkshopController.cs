using Interactors;
using Data;
using System.Linq;

public class WorkshopController : Controller {

    public WorkshopMenu workshopMenu;
    public BlackFade blackFade;

    public Args args { private get; set; }
    public OpenWorkshopInteractor openWorkshopInteractor { private get; set; }
    public AnalyseItemInteractor analyseItemInteractor { private get; set; }
    public ScrapItemInteractor scrapItemInteractor { private get; set; }
    public BuildItemInteractor buildItemInteractor { private get; set; }

    private WorkshopItem currentItem;

    public void InitPage() {
        openWorkshopInteractor.Interact(new OpenWorkshopInput {});
    }

    public void SelectInventoryItem(long itemId) {
        if (disabled) return;
        currentItem = FindItem(itemId);
        workshopMenu.SelectInventoryItem(currentItem);
    }

    public void SelectBlueprintItem(string itemName) {
        if (disabled) return;
        currentItem = FindBlueprint(itemName);
        workshopMenu.SelectBlueprintItem(currentItem);
    }

    public void AnalyseCurrentItem() {
        if (disabled) return;
        analyseItemInteractor.Interact(new AnalyseItemInput {
            itemId = currentItem.itemId
        });
    }

    public void ScrapCurrentItem() {
        if (disabled) return;
        scrapItemInteractor.Interact(new ScrapItemInput {
            itemId = currentItem.itemId
        });
    }

    public void BuildCurrentItem() {
        if (disabled) return;
        buildItemInteractor.Interact(new BuildItemInput {
            itemName = currentItem.itemName
        });
    }

    public void GoBack() {
        if (!disabled) {
            blackFade.BeginFade(() => {
                ArmouryInitializer.OpenScene();
            });
        }
    }

    private WorkshopItem FindItem(long itemId) {
        return args.items.First(item => item.itemId == itemId);
    }

    private WorkshopItem FindBlueprint(string itemName) {
        return args.blueprints.First(item => item.itemName == itemName);
    }

    public struct Args {
        public WorkshopItem[] items;
        public WorkshopItem[] blueprints;
    }
}
