using UnityEngine;
using TMPro;

public class AlienInfoCampaignComponent : MonoBehaviour {
    
    public static AlienInfoCampaignComponent instance;
    
    public TMP_Text infoText;
    
    void Awake() => instance = this;
    
    void Start() {
        Close();
    }
    
    public void ShowAlien(AlienData alien) {
        infoText.text = $"{alien.name}\n\n{alien.description}";
        Show();
    }
    
    public void Show() {
        gameObject.SetActive(true);
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
}