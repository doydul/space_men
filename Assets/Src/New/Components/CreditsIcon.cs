using UnityEngine;
using TMPro;

public class CreditsIcon : MonoBehaviour {
    
    public TMP_Text text;

    public void SetCredits(int credits) {
        text.text = credits.ToString() + "\ncreds";
    }
}