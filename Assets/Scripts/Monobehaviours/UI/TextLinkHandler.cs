using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler {
    
    TMP_Text textElement;
    
    void Awake() {
        textElement = GetComponent<TMP_Text>();
    }
    
    public void OnPointerClick(PointerEventData eventData) {
		var linkIndex = TMP_TextUtilities.FindIntersectingLink(textElement, Input.mousePosition, null);
        
        if (linkIndex != -1) {
            Tutorial.ShowTooltip(textElement.textInfo.linkInfo[linkIndex].GetLinkID());
        }
	}
}