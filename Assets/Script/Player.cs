using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public static Player Instance;
    Rigidbody2D rigid;
    [SerializeField] SpriteRenderer rend;


    [Header("Player")]
    public float moveSpeed;
    public float jumpPower;
    public int jumpCount = 0;

    [Header("Bullet")]
    public Transform bulletPos;
    public GameObject bulletPrefabs;
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
        dir = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigid.velocity = new Vector2(-1 * moveSpeed, rigid.velocity.y);
            rend.flipX = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigid.velocity = new Vector2(1 * moveSpeed, rigid.velocity.y);
            rend.flipX = false;
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
            ObjectPoolManager.SpawnFromPool("Bullet", bulletPos.position, transform);
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
