using Data;

public class ScrapItemPresenter : Presenter, IPresenter<ScrapItemOutput> {
  
    public static ScrapItemPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public WorkshopMenu menu;
    
    public void Present(ScrapItemOutput input) {
        menu.DisplayItems(input.state);
    }
}

