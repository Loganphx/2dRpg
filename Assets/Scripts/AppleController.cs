using Items;
using UnityEngine;

public class AppleController: MonoBehaviour
{
    public Item apple;
    public ItemDatabase itemDatabase;
    public void Start()
    {
        itemDatabase = GameObject.Find("Item Database").GetComponent<ItemDatabase>();
        apple = itemDatabase.GetItem("Apple");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (apple.maxLife.Equals(0) || !other.gameObject.CompareTag("Player")) return;
        if (apple.maxLife % 1 == 0)
        {
            other.gameObject.GetComponent<HeartController>().MaxHealth(apple.maxLife);
        }
    }
}