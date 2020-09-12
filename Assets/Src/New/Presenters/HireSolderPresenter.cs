using Data;
using TMPro;

public class HireSolderPresenter : Presenter, IPresenter<HireSolderOutput> {
  
    public static HireSolderPresenter instance { get; private set; }

    public TMP_Text creditText;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(HireSolderOutput input) {
        creditText.text = input.newCreditBalance.ToString();
    }
}

