using Data;

public class OpenArmouryPresenter : Presenter, IPresenter<OpenArmouryOutput> {
    
    public BlackFade blackFade;

    public static OpenArmouryPresenter instance { get; private set; }
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenArmouryOutput input) {
        blackFade.BeginFade(() => {
            ArmouryInitializer.OpenScene(new ArmouryMenuArgs {
                soldierInfo = input.squadSoldiers
            });
        });
    }
}

