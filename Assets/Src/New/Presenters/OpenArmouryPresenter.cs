using Data;
using TMPro;

public class OpenArmouryPresenter : Presenter, IPresenter<OpenArmouryOutput> {

    public static OpenArmouryPresenter instance { get; private set; }

    public TMP_Text creditsText;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(OpenArmouryOutput input) {
        FindObjectOfType<ArmouryMenu>().Init(input.squadSoldiers);
        creditsText.text = input.credits.ToString();
    }
}

