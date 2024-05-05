using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemElement : MonoBehaviour {
    public InventoryItem item { get; set; }

    public Button button;
    public UnityEvent<InventoryItem> onClick;

    void Awake() {
        button.onClick.AddListener(() => onClick.Invoke(item));
    }
}