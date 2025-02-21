using UnityEngine;
using TMPro;

public class AlienInfoCampaignComponent : MonoBehaviour {
    
    public static AlienInfoCampaignComponent instance;
    
    public TMP_Text infoText;
    
    void Awake() => instance = this;
    
    void Start() => Close();
    
    public void ShowAlien(AlienData alien) {
        infoText.text = $"{alien.name}<size=80%>\n\n<align=\"left\">{alien.description}\nhp: {alien.maxHealth}\nmovement: {alien.movement}\ndamage: {alien.damage}\narmour: {alien.armour}";
        Show();
    }
    
    public void Show() {
        gameObject.SetActive(true);
    }
    
    public void Close() {
        gameObject.SetActive(false);
    }
}