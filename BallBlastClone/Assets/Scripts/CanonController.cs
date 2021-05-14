using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool Move;
    private Vector2 position;
    public float Speed;
    private Vector2 screenBounds;
    private float objectWidth;
    public GameObject LeftWheel;
    public GameObject RightWheel;
    private float velocity;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        position=rb.position;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<PolygonCollider2D>().bounds.extents.x; //extents = size of width / 2
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetMouseButton(0);
        if (Move)
        {
            position.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        }
    }

    private void FixedUpdate()
    {
        if (Move)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,position,Speed*Time.fixedDeltaTime);
            velocity = gameObject.transform.position.x-position.x / Time.deltaTime;
            LeftWheel.transform.rotation = Quaternion.Euler(0,0,velocity);
            RightWheel.transform.rotation = Quaternion.Euler(0, 0, velocity*1.1f);
        }
        
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        transform.position = viewPos;
    }
}
