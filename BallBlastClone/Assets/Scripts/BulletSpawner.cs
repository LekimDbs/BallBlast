using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    Queue<GameObject> bulletQueue;
    public GameObject bulletPrefab;
    public int bulletCount;

    public float BPS;
    public float BulletSpeed;
    private float nextFire;

    GameObject go;

    public static BulletSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PrepareBullets();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0)&&Time.time>nextFire)
        {
            nextFire = Time.time + 1 / BPS;
             go = SpawnBullets(transform.position);
             if (go != null)
                {
                    go.GetComponent<Rigidbody2D>().velocity = Vector2.up * BulletSpeed;
                }
            
        }
    }
    public void PrepareBullets()
    {
        bulletQueue = new Queue<GameObject>();
        for (int i = 0; i < bulletCount; i++)
        {
            go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            go.SetActive(false);
            bulletQueue.Enqueue(go);
        }
    }

    GameObject SpawnBullets(Vector2 position)
    {
        if (bulletQueue.Count > 0)
        {
            go = bulletQueue.Dequeue();
            go.transform.position = position;
            float random = Random.Range(0f, 1f);
            if (random < (BuyManager.FirePower - 1) % 1) {
                go.GetComponent<Bullet>().damage += 1;
                go.GetComponent<SpriteRenderer>().color = Color.green;
            }
            go.SetActive(true);
            return go;
        }
        return null;
    }

    

    public void DestroyBullet(GameObject bullet)
    {
        bullet.GetComponent<Bullet>().damage = 1;
        bulletQueue.Enqueue(bullet);
        bullet.SetActive(false);
    }
}
