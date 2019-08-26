using UnityEngine;
using TMPro;

public class SelectedItemInfoPresenter : Presenter, IPresenter<SelectedItemInfoPresenterInputData> {
  
    public GameObject info;
    public TextMeshProUGUI infoText;
    
    public void Present(SelectedItemInfoPresenterInputData dataObject) {
        infoText.text = dataObject.infoText;
        if (dataObject.showInfoPanel) {
            ShowInfoPanel();
        } else {
            HideInfoPanel();
        }
    }
    
    void Start() {
        HideInfoPanel();
    }
    
    void ShowInfoPanel() {
        info.SetActive(true);
    }
    
    void HideInfoPanel() {
        info.SetActive(false);
    }
    
    void SetInfoText(string text) {
        infoText.text = text;
    }
}
