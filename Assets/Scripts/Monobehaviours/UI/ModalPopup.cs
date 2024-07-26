using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class ModalPopup : MonoBehaviour {

    public Transform container;

    Action onOk;
    
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

    public void OnOk(Action callback) => onOk = callback;

    public void HandleOk() {
        if (onOk != null) {
            onOk();
            onOk = null;
        } else {
            Hide();
        }
    }

    public void DisplayEOL() {
        ClearContent();
        DisplayContent(Resources.Load<GameObject>("Prefabs/UI/MissionCompleteNotification"));
        OnOk(() => {
            Campaign.NextLevel(PlayerSave.current);
            SceneManager.LoadScene("CampaignUI");
        });
    }
}