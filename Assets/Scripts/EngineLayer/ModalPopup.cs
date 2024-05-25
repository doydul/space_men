using UnityEngine;
using System.Collections.Generic;

public class ModalPopup : MonoBehaviour {

    public Transform container;
    
    public static ModalPopup instance;
    void Awake() => instance = this;
    void Start() => Hide();

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void ClearContent() {
        var children = new List<GameObject>();
        foreach (Transform child in container) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public GameObject DisplayContent(GameObject prefab) => DisplayContent(prefab.transform);
    public GameObject DisplayContent(Transform prefab) {
        Show();
        return Instantiate(prefab, container).gameObject;
    }

    public void HandleOk() {
        Hide();
    }
}