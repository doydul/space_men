using Data;

public class AnalyseItemPresenter : Presenter, IPresenter<AnalyseItemOutput> {
  
    public static AnalyseItemPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public WorkshopMenu menu;
    
    public void Present(AnalyseItemOutput input) {
        menu.DisplayItems(input.state);
    }
}

