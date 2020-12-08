using System;
using UnityEngine;


public class Heart : MonoBehaviour
{
    public float amount;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite activeSprite;

    /// <summary>
    /// Instantiates a new Heart.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="fullHeart"></param>
    /// <param name="halfHeart"></param>
    /// <param name="emptyHeart"></param>
    public Heart(float amount, Sprite fullHeart, Sprite halfHeart, Sprite emptyHeart)
    {
        this.amount = amount;
        this.fullHeart = fullHeart;
        this.halfHeart = halfHeart;
        this.emptyHeart = emptyHeart;
    }
    
    /// <summary>
    /// Duplicates a heart.
    /// </summary>
    /// <param name="heart"></param>
    /// 
    public Heart(Heart heart)
    {
        this.amount = heart.amount;
        this.fullHeart = heart.fullHeart;
        this.halfHeart = heart.halfHeart;
        this.emptyHeart = heart.emptyHeart;
    }

    public void Start()
    {
        fullHeart = Resources.Load<Sprite>("Sprites/fullHeart");
        halfHeart = Resources.Load<Sprite>("Sprites/halfHeart");
        emptyHeart = Resources.Load<Sprite>("Sprites/emptyHeart");

    }
}
