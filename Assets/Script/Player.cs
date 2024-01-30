using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;


    [Header("Player")]
    public float moveSpeed;
    public float jumpPower;
    public int jumpCount = 0;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPos;
    [SerializeField] GameObject bullet;
    public float bulletSpeed;
    Vector3 dir;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Fire();
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigid.velocity = new Vector2(-1 * moveSpeed, rigid.velocity.y);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigid.velocity = new Vector2(1 * moveSpeed, rigid.velocity.y);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount >= 1)
        {
            rigid.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            jumpCount--;
        }
    }

    public void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet);
            bullet.transform.position = bulletPos.transform.position;
            bullet.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
            //Instantiate(bullet, bulletPos.transform.position, transform.rotation);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ground"))
        {
            jumpCount++;
        }
    }
}
