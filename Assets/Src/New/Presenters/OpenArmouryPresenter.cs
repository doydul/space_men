using Data;

public class OpenArmouryPresenter : Presenter, IPresenter<OpenArmouryOutput> {

    public static OpenArmouryPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenArmouryOutput input) {
        FindObjectOfType<ArmouryMenu>().Init(input.squadSoldiers);
    }
}

