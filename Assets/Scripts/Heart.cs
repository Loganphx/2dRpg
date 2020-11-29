using UnityEngine;


public class Heart : MonoBehaviour
{
    public float amount;
    public Sprite icon;

    /// <summary>
    /// Instantiates a new Heart.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="icon"></param>
    public Heart(float amount, Sprite icon)
    {
        this.amount = amount;
        this.icon = icon;
    }
    
    /// <summary>
    /// Duplicates a heart.
    /// </summary>
    /// <param name="heart"></param>
    /// 
    public Heart(Heart heart)
    {
        this.amount = heart.amount;
        this.icon = heart.icon;
    }
}
