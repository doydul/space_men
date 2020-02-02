using Data;

public class OpenArmourSelectPresenter : Presenter, IPresenter<OpenArmourSelectOutput> {
  
    public static OpenArmourSelectPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenArmourSelectOutput input) {
        // TODO
    }
}

