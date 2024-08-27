using UnityEngine;

public class Settings {
    
    public static bool confirmAbilities {
        get => PlayerPrefs.HasKey("confirm_abilities") ? PlayerPrefs.GetInt("confirm_abilities") == 1 : true;
        set => PlayerPrefs.SetInt("confirm_abilities", value ? 1 : 0);
    }
}