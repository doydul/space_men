using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Tutorial {
    
    class TutorialQueueItem {
        public Transform target;
        public string tutorialName;
        public bool offset;
    }
    
    const string popupResourcePath = "Prefabs/UI/TutorialPopup";
    const string tutorialTextResourcePath = "Prefabs/UI/Tutorials";
    const int borderWidth = 200;
    
    static Queue<TutorialQueueItem> queue = new();
    static bool tutorialInProgress;
    
    public static TutorialText tutorialText => Resources.Load<TutorialText>(tutorialTextResourcePath);
    
    public static void ResetAll() {
        PlayerPrefs.DeleteKey("tutorials");
    }
    
    public static bool Shown(string tutorialName) {
        if (!PlayerPrefs.HasKey("tutorials")) return false;
        return PlayerPrefs.GetString("tutorials").Contains(tutorialName);
    }
    
    public static void Record(string tutorialName) {
        if (Shown(tutorialName)) return;
        if (!PlayerPrefs.HasKey("tutorials")) {
            PlayerPrefs.SetString("tutorials", tutorialName);
            return;
        }
        PlayerPrefs.SetString("tutorials", $"{PlayerPrefs.GetString("tutorials")}:{tutorialName}");
    }
    
    public static void Show(Transform target, string tutorialName, bool offset = true, bool ui = false) {
        if (!Shown(tutorialName)) {
            if (tutorialInProgress) {
                queue.Enqueue(new TutorialQueueItem { target = target, tutorialName = tutorialName, offset = offset });
            } else {
                Debug.Log(tutorialName);
                tutorialInProgress = true;
                Record(tutorialName);
                var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
                var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
                var popup = trans.GetComponent<TutorialPopup>();
                var parentRectTrans = parent as RectTransform;
                popup.SetText(tutorialText.Get(tutorialName));
                Vector2 screenPos;
                if (ui) {
                    screenPos = target.position;
                } else {
                    screenPos = Camera.main.WorldToScreenPoint(target.position); 
                    if (screenPos.x > parentRectTrans.rect.width * parentRectTrans.localScale.x || screenPos.x < 0 || screenPos.y > parentRectTrans.rect.height * parentRectTrans.localScale.y || screenPos.y < 0) {
                        CameraController.CentreCameraOn(target, true);
                        screenPos = Camera.main.WorldToScreenPoint(target.position);
                    }
                }
                if (offset) {
                    if (screenPos.y < borderWidth * parentRectTrans.localScale.y) popup.ApplyOffset(TutorialPopup.Offset.Above);
                    else if (screenPos.y > (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y) popup.ApplyOffset(TutorialPopup.Offset.Below);
                    else if (screenPos.x > parentRectTrans.rect.width * parentRectTrans.localScale.x / 2) popup.ApplyOffset(TutorialPopup.Offset.Left);
                    else popup.ApplyOffset(TutorialPopup.Offset.Right);
                }
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, parent.GetComponent<Canvas>().worldCamera, out var movePos);
                var targetPos = parent.TransformPoint(movePos);
                trans.position = targetPos;
                
                var panelPos = popup.panel.position;
                if (panelPos.x < borderWidth * parentRectTrans.localScale.x) panelPos.x = borderWidth * parentRectTrans.localScale.x;
                if (panelPos.x > (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x) panelPos.x = (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x;
                if (panelPos.y < borderWidth * parentRectTrans.localScale.y) panelPos.y = borderWidth * parentRectTrans.localScale.y;
                if (panelPos.y > (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y) panelPos.y = (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y;
                popup.panel.position = panelPos;
            }
        }
    }
    
    public static void Show(string tutorialName) {
        if (!Shown(tutorialName)) {
            if (tutorialInProgress) {
                queue.Enqueue(new TutorialQueueItem { tutorialName = tutorialName });
            } else {
                tutorialInProgress = true;
                Record(tutorialName);
                var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
                var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
                var popup = trans.GetComponent<TutorialPopup>();
                var parentRectTrans = parent as RectTransform;
                popup.SetText(tutorialText.Get(tutorialName));
                var screenPos = new Vector2(parentRectTrans.rect.width * parentRectTrans.localScale.x / 2, parentRectTrans.rect.height * parentRectTrans.localScale.y / 2);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, parent.GetComponent<Canvas>().worldCamera, out var movePos);
                var targetPos = parent.TransformPoint(movePos);
                trans.position = targetPos;
            }
        }
    }
    
    public static void Finished() {
        tutorialInProgress = false;
        if (queue.Any()) {
            var next = queue.Dequeue();
            if (next.target != null) Show(next.target, next.tutorialName, next.offset);
            else Show(next.tutorialName);
        }
    }
}