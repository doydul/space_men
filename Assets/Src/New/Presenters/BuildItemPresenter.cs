using Data;

public class BuildItemPresenter : Presenter, IPresenter<BuildItemOutput> {
  
    public static BuildItemPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public WorkshopMenu menu;
    
    public void Present(BuildItemOutput input) {
        menu.DisplayItems(input.state);
    }
}

