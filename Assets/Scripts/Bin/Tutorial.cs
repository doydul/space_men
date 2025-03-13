using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

public class Tutorial {
    
    class TutorialQueueItem {
        public Transform target;
        public string tutorialName;
        public bool offset;
        public bool ui;
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
            var tutorial = new TutorialQueueItem { target = target, tutorialName = tutorialName, offset = offset, ui = ui };
            if (tutorialInProgress) {
                queue.Enqueue(tutorial);
            } else {
                Debug.Log(tutorialName);
                tutorialInProgress = true;
                Record(tutorialName);
                MakeTutorial(tutorial);
            }
        }
    }
    
    public static void Show(string tutorialName) {
        if (!Shown(tutorialName)) {
            var tutorial = new TutorialQueueItem { tutorialName = tutorialName };
            if (tutorialInProgress) {
                queue.Enqueue(tutorial);
            } else {
                tutorialInProgress = true;
                Record(tutorialName);
                MakeTutorial(tutorial);
            }
        }
    }
    
    public static void ShowTooltip(string tutorialName) {
        MakeTutorial(new TutorialQueueItem { tutorialName = tutorialName });
    }
    
    public static void Finished() {
        tutorialInProgress = false;
        if (queue.Any()) {
            var next = queue.Dequeue();
            if (next.target != null) Show(next.target, next.tutorialName, next.offset, next.ui);
            else Show(next.tutorialName);
        }
    }
    
    static void MakeTutorial(TutorialQueueItem tutorial) {
        if (tutorial.target != null) {
            var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
            var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
            var popup = trans.GetComponent<TutorialPopup>();
            var parentRectTrans = parent as RectTransform;
            popup.SetText(tutorialText.Get(tutorial.tutorialName));
            Vector2 screenPos;
            if (tutorial.ui) {
                screenPos = tutorial.target.position;
            } else {
                screenPos = Camera.main.WorldToScreenPoint(tutorial.target.position); 
                if (screenPos.x > parentRectTrans.rect.width * parentRectTrans.localScale.x || screenPos.x < 0 || screenPos.y > parentRectTrans.rect.height * parentRectTrans.localScale.y || screenPos.y < 0) {
                    CameraController.CentreCameraOn(tutorial.target, true);
                    screenPos = Camera.main.WorldToScreenPoint(tutorial.target.position);
                }
            }
            if (tutorial.offset) {
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
        } else {
            var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
            var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
            var popup = trans.GetComponent<TutorialPopup>();
            var parentRectTrans = parent as RectTransform;
            popup.SetText(tutorialText.Get(tutorial.tutorialName));
            var screenPos = new Vector2(parentRectTrans.rect.width * parentRectTrans.localScale.x / 2, parentRectTrans.rect.height * parentRectTrans.localScale.y / 2);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, parent.GetComponent<Canvas>().worldCamera, out var movePos);
            var targetPos = parent.TransformPoint(movePos);
            trans.position = targetPos;
        }
    }
}