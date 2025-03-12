using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemListElement : MonoBehaviour {
    
    public Image highlight;
    public Image blueprintBackground;
    
    bool _isBlueprint;
    public bool isBlueprint {
        get => _isBlueprint;
        set {
            _isBlueprint = value;
            blueprintBackground.enabled = isBlueprint;
        }
    }
    
    void Start() {
        Deselect();
    }
    
    public void Select() {
        highlight.enabled = true;
        if (!isBlueprint && !Tutorial.Shown("inventory_item1")) {
            StartCoroutine(ShowTutorials());
        }
    }
    
    IEnumerator ShowTutorials() {
        yield return null;
        Tutorial.Show(GameObject.Find("ItemInfoPanel").transform, "inventory_item1", true, true);
        Tutorial.Show(GameObject.Find("ScrapButton").transform, "inventory_item2", true, true);
        Tutorial.Show(GameObject.Find("ResearchButton").transform, "inventory_item3", true, true);
    }
    
    public void Deselect() {
        highlight.enabled = false;
    }
}