using UnityEngine;
using UnityEngine.Events;

public class TutorialPanel : MonoBehaviour {

    public UnityEvent OnClose;

    public bool shown { get; private set; }
    
    void Awake() {
        Hide();
    }

    void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        shown = true;
    }

    public void Close() {
        OnClose.Invoke();
        Hide();
    }
}