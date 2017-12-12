using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIHeart : MonoBehaviour {

    public Sprite alive;
    public Sprite dead;

    private Image image;

    private void OnEnable() {
        image = GetComponent<Image>();
        image.sprite = alive;
    }

    public bool isAlive {
        set { GetComponent<Image>().sprite = value ? alive : dead; }
    }
}
