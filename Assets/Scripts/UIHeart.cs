using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIHeart : MonoBehaviour
{
    public Heart heart;
    private Image _spriteImage;
    private UIHeart _selectedHeart;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _spriteImage = GetComponent<Image>();
        UpdateHeart(heart);
        _selectedHeart = GameObject.Find("SelectedItem").GetComponent<UIHeart>();

    }

    /// <summary>
    /// Updates item's sprite and color
    /// </summary>
    /// <param name="item"></param>
    public void UpdateHeart(Heart heart)
    {
        this.heart = heart;
        if (this.heart != null)
        {
            _spriteImage.color = new Color(255, 255, 255, 255);
            _spriteImage.sprite = this.heart.icon;
            return;
        }

        _spriteImage.color = Color.clear;
    }
}
