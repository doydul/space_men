using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Tutorial : MonoBehaviour {
    
    const string popupResourcePath = "Prefabs/UI/TutorialPopup";
    const string tutorialTextResourcePath = "Prefabs/UI/Tutorials";
    const int borderWidth = 200;
    
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
    
    public static void Show(Transform target, string tutorialName) {
        if (!Shown(tutorialName)) {
            Record(tutorialName);
            var parent = Object.FindObjectsOfType<Canvas>().First(canv => canv.gameObject.name != "Backdrop").transform;
            var trans = Object.Instantiate(Resources.Load<Transform>(popupResourcePath), parent);
            var popup = trans.GetComponent<TutorialPopup>();
            var parentRectTrans = parent as RectTransform;
            popup.SetText(tutorialText.Get(tutorialName));
            var screenPos = Camera.main.WorldToScreenPoint(target.position);
            if (screenPos.x < borderWidth * parentRectTrans.localScale.x) screenPos.x = borderWidth * parentRectTrans.localScale.x;
            if (screenPos.x > (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x) screenPos.x = (parentRectTrans.rect.width - borderWidth) * parentRectTrans.localScale.x;
            if (screenPos.y < borderWidth * parentRectTrans.localScale.y) screenPos.y = borderWidth * parentRectTrans.localScale.y;
            if (screenPos.y > (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y) screenPos.y = (parentRectTrans.rect.height - borderWidth) * parentRectTrans.localScale.y;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, parent.GetComponent<Canvas>().worldCamera, out var movePos);
            var targetPos = parent.TransformPoint(movePos);
            trans.position = targetPos;
        }
    }
}