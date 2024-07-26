using UnityEngine;
using TMPro;

public class ItemReward : MonoBehaviour {

    public TMP_Text textElement;

    public void SetText(string text) => textElement.text = text;
}