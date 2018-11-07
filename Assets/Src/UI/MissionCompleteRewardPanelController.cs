using UnityEngine;
using UnityEngine.UI;

public class MissionCompleteRewardPanelController : MonoBehaviour {

    public Image image;

    public void SetSprite(Sprite sprite) {
        image.sprite = sprite;
    }

    void Start() {
        if (image.sprite == null) gameObject.SetActive(false);
    }
}
