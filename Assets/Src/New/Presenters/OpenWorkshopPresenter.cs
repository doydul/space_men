using UnityEngine;
using Data;

public class OpenWorkshopPresenter : Presenter, IPresenter<OpenWorkshopOutput> {
  
    public static OpenWorkshopPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }

    public WorkshopMenu menu;
    
    public void Present(OpenWorkshopOutput input) {
        menu.DisplayItems(input.state);
    }
}

