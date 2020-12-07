using System;
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
        if(Math.Abs(this.heart.amount - 1f) < 0.05)
        {
            _spriteImage.color = new Color(255, 255, 255, 255);
            _spriteImage.sprite = this.heart.fullHeart;
            this.heart.activeSprite = this.heart.fullHeart;
            return;
        }

        if (Math.Abs(this.heart.amount - 0.5) < 0.05)
        {
            _spriteImage.color = new Color(255, 255, 255, 255);
            _spriteImage.sprite = this.heart.halfHeart;
            return;
        }

        if (this.heart.amount == 0.0)
        {
            _spriteImage.color = new Color(255, 255, 255, 255);
            _spriteImage.sprite = this.heart.emptyHeart;
            return;
        }
    }
}
