using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    public GameObject SmallerTarget;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public int health;
    public SpriteRenderer sr;

    [SerializeField] public TMP_Text textHealth;
    [SerializeField] public float jumpForce;
    private int mainHealth;

    private float[] leftAndRight = new float[2] { -1f, 1f };
    public bool Splittable = true;
    [HideInInspector]public bool NotMain;
    private bool isNotShowing;

    private float red;
    private float blue;
    private float green;
    private bool ColorChanged;

    private bool over;

    void Start()
    {
        SetStartColor();
        mainHealth = health;
        UpdateHealth();
        if (NotMain==true)
        {
            FallDown();
        }
        else
        {
            isNotShowing = true;
            rb.gravityScale = 0f;
            float direction = leftAndRight[Random.Range(0, 2)];
            float screenOffset = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 1.3f;
            transform.position = new Vector2(screenOffset * direction, transform.position.y);
            rb.velocity = new Vector2(-direction, 0f);
            Invoke("FallDown", Random.Range(screenOffset - 3.5f, screenOffset - 1.5f));
        }

    }
   
    void FallDown()
    {
        isNotShowing = false;
        rb.gravityScale = 1f;
        rb.AddTorque(Random.Range(-20f, 20f));
    }

    void SetStartColor()
    {

        if (health > BulletSpawner.Instance.BPS * 4)
        {
            red = 255;
            blue = 0;
            green = 0;
        }
        else if (health > BulletSpawner.Instance.BPS * 2)
        {
            red = 255;
            blue = 255;
            green = 0;
        }
        else if (health > BulletSpawner.Instance.BPS * 1.5f)
        {
            red = 0;
            blue = 255;
            green = 0;
        }
        else if (health > BulletSpawner.Instance.BPS * 1.2f)
        {
            red = 0;
            blue = 255;
            green = 255;
        }
        else if (health > BulletSpawner.Instance.BPS)
        {
            red = 0;
            blue = 0;
            green = 255;
        }
        else
        {
            red = 0;
            blue = 0;
            green = 255;
        }
        sr.color = new Color(red/255f,green/255f,blue/255f);
    }

    void GameOver(Collider2D other)
    {
        StartCoroutine(SetTime());
        Time.timeScale = 0;
        other.GetComponent<Animator>().SetBool("isDead", true);
        textHealth.GetComponent<Animator>().SetBool("isDead", true);
        other.GetComponent<CanonController>().enabled = false;
        other.GetComponent<PolygonCollider2D>().enabled = false;
        other.transform.GetChild(2).transform.gameObject.SetActive(false);
        UIManager.Instance.GameOver();
        UIManager.Instance.SetScore(TargetSpawner.score);
        UIManager.Instance.CurrentScoreText.gameObject.SetActive(false);
        float perc = TargetSpawner.deadCount / TargetSpawner.TotalCount * 100;
        UIManager.Instance.SetPercentage(perc);
        StartCoroutine(SetOver());
        
    }
    IEnumerator SetOver()
    {
        yield return new WaitForSeconds(0.68f);
        over = true;
    }
    private void Update()
    {
        if (over)
        {
            float step = 20f * Time.deltaTime;
            UIManager.Instance.ScoreBoard.transform.position = Vector2.MoveTowards(UIManager.Instance.ScoreBoard.transform.position, UIManager.Instance.SliderPos.transform.position, step);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Canon"))
        {
            GameOver(other);
        }

        if (other.tag.Equals("Bullet"))
        {
            TakeDamage((int)other.GetComponent<Bullet>().damage);
            BulletSpawner.Instance.DestroyBullet(other.gameObject);

        }

        if (!isNotShowing && other.tag.Equals("Wall"))
        {
            float posX = transform.position.x;
            if (posX > 0)
            {
                rb.AddForce(Vector2.left * 150f);
            }
            else
            {
                rb.AddForce(Vector2.right * 150f);
            }

            rb.AddTorque(posX * 4f);
        }

        if (other.tag.Equals("Ground"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddTorque(-rb.angularVelocity * 4f);
        }
    }
   
    IEnumerator SetTime()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        Time.timeScale = 1;
    }
    public void TakeDamage(int damage)
    {
        float healthperc = health * mainHealth / 100;
        if (blue<255 && ColorChanged==false)
        {
            blue += BulletSpawner.Instance.BPS * 2;
             sr.color = new Color(sr.color.r, sr.color.g, Mathf.Clamp(blue, 0, 255f) / 255f);
        }
        else if (red>0)
        {
            if(ColorChanged==false)
            ColorChanged = true;
            red -= BulletSpawner.Instance.BPS * 2;
            sr.color = new Color(Mathf.Clamp(red, 0, 255f) / 255f, sr.color.g, sr.color.b);
        }
        else if (green<255)
        {
            green += BulletSpawner.Instance.BPS * 2;
            sr.color = new Color(sr.color.r, Mathf.Clamp(green, 0, 255f) / 255f, sr.color.b);
        }
        else if (blue>0)
        {
            blue -= BulletSpawner.Instance.BPS * 2f;
            sr.color = new Color(sr.color.r, sr.color.g, Mathf.Clamp(blue, 0, 255f) / 255f);
        }
        
        if (health > damage)
        {
            health -= damage;
        }
        else
        {
            Die();
        }
        TargetSpawner.score += damage;
        UIManager.Instance.SetCurrentScore(TargetSpawner.score);
        UpdateHealth();
    }

    virtual protected void Die()
    {
        if (Splittable)
        {
            Split();
        }
        TargetSpawner.Instance.ActiveTargets.Remove(gameObject);
        TargetSpawner.deadCount+=1;
        if (TargetSpawner.deadCount == TargetSpawner.TotalCount)
        {
            UIManager.Instance.GameWon();
        }
        float perc = TargetSpawner.deadCount / TargetSpawner.TotalCount * 100;
        UIManager.Instance.SliderPercent = perc;
        float random = Random.Range(0f, 1f);
        if (random > 0.5f)
        {
            GameObject go = Instantiate(TargetSpawner.Instance.Coin,gameObject.transform.position,Quaternion.identity);
            float random2 = Random.Range(0f, 1f);
            if (random2 < (BuyManager.CoinsPower - 1) % 1)
            {
                go.GetComponent<Coin>().value += 1;
                go.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

        Destroy(gameObject);
    }

    protected void UpdateHealth()
    {
        textHealth.text = health.ToString();
    }

    public void Split()
    {
        GameObject go;
        for (int i = 0; i < 2; i++)
        {
            go = Instantiate(SmallerTarget, transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(leftAndRight[i], 2f);
            go.GetComponent<Target>().NotMain = true;
            go.GetComponent<SpriteRenderer>().sortingOrder = TargetSpawner.Instance.layerorder;
            TargetSpawner.Instance.layerorder += 1;
            go.GetComponent<Target>().textHealth.GetComponent<MeshRenderer>().sortingOrder = TargetSpawner.Instance.layerorder;
            TargetSpawner.Instance.layerorder += 1;
            int newHealth = mainHealth / 2;
            go.GetComponent<Target>().health = Mathf.Clamp(newHealth, 1, mainHealth / 2);
        }
    }
}

