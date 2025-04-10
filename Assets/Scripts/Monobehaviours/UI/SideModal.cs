using UnityEngine;
using TMPro;

public class SideModal : MonoBehaviour {

    public TMP_Text modalText;
    
    public static SideModal instance;
    
    public GameObject modalElement;
    public GameObject collapseButton;
    public GameObject collapseButtonImage;
    
    public bool isOpen { get; private set; }
    public bool collapsed {
        get => PlayerPrefs.GetInt("side_modal_collapsed") > 0;
        set => PlayerPrefs.SetInt("side_modal_collapsed", value ? 1 : 0);
    }
    
    void Awake() => instance = this;
    void Start() {
        collapseButtonImage.transform.localScale = new Vector3(1, collapsed ? -1 : 1, 1);
        Hide();
    }
    
    public void Show(string text) {
        modalElement.SetActive(true);
        modalText.text = text;
        isOpen = true;
    }
    
    public void ShowCollapsible(string text) {
        collapseButton.SetActive(true);
        modalElement.SetActive(!collapsed);
        modalText.text = text;
        isOpen = true;
    }
    
    public void ToggleCollapse() {
        collapsed = !collapsed;
        modalElement.SetActive(!collapsed);
        collapseButtonImage.transform.localScale = new Vector3(1, collapsed ? -1 : 1, 1);
    }
    
    public void Hide() {
        modalElement.SetActive(false);
        collapseButton.SetActive(false);
        isOpen = false;
    }
}