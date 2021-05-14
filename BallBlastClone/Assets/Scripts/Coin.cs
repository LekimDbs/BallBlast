using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    public Rigidbody2D rb;

    private void Start()
    {
        value = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("CoinGround")) {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        if (collision.transform.tag.Equals("Canon"))
        {
            BuyManager.Instance.AddCoins(value);
            Destroy(this.gameObject);
        }
    }
}
