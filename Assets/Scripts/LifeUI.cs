using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField]private Sprite lifeUp;
    [SerializeField]private Sprite lifeDown;

    private Image img;
    public void Awake() {
        img = GetComponent<Image>();
        if (lifeUp != null)
            img.sprite = lifeUp;
        else if (img != null)
            img.color = new Color(0,255, 0);
    }

    public void LifeUp() {
        if (lifeUp != null)
            img.sprite = lifeUp;
        else if (img != null)
            img.color = new Color(0,255, 0);
    }
    public void LifeDown() {
        if (lifeDown != null)
            img.sprite = lifeDown;
        else if (img != null)
            img.color = new Color(255,0, 0);
    }
}