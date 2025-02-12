using UnityEngine;
using System.Linq;

public class Tutorial {
    
    const string popupResourcePath = "Prefabs/UI/TutorialPopup";
    const string tutorialTextResourcePath = "Prefabs/UI/Tutorials";
    const int borderWidth = 200;
    const int holeOffset = 300;
    
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
    
    public static void Show(Transform target, string tutorialName, bool offset = true) {
        if (!Shown(tutorialName)) {
            Record(tutorialName);
            var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
            var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
            var popup = trans.GetComponent<TutorialPopup>();
            var parentRectTrans = parent as RectTransform;
            popup.SetText(tutorialText.Get(tutorialName));
            var screenPos = Camera.main.WorldToScreenPoint(target.position);
            if (screenPos.x > parentRectTrans.rect.width * parentRectTrans.localScale.x || screenPos.x < 0 || screenPos.y > parentRectTrans.rect.height * parentRectTrans.localScale.y || screenPos.y < 0) {
                CameraController.CentreCameraOn(target, true);
                screenPos = Camera.main.WorldToScreenPoint(target.position);
            }
            if (offset) {
                if (screenPos.y < borderWidth * parentRectTrans.localScale.y) popup.ApplyOffset(TutorialPopup.Offset.Above);
                else if (screenPos.y > (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y) popup.ApplyOffset(TutorialPopup.Offset.Below);
                else if (screenPos.x > parentRectTrans.rect.width * parentRectTrans.localScale.x / 2) popup.ApplyOffset(TutorialPopup.Offset.Left);
                else popup.ApplyOffset(TutorialPopup.Offset.Right);
            } else {
                if (screenPos.x < borderWidth * parentRectTrans.localScale.x) screenPos.x = borderWidth * parentRectTrans.localScale.x;
                if (screenPos.x > (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x) screenPos.x = (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x;
                if (screenPos.y < borderWidth * parentRectTrans.localScale.y) screenPos.y = borderWidth * parentRectTrans.localScale.y;
                if (screenPos.y > (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y) screenPos.y = (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y;
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, parent.GetComponent<Canvas>().worldCamera, out var movePos);
            var targetPos = parent.TransformPoint(movePos);
            trans.position = targetPos;
            
        }
    }
}