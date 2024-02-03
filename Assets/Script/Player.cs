using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] SpriteRenderer rend;


    [Header("Player")]
    public float moveSpeed;
    public float jumpPower;
    public int jumpCount = 0;

    [Header("Bullet")]
    public GameObject bulletPos;
    public GameObject prefabBullet;
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
        dir = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, -cam.transform.position.z));
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
            GameObject bullet = Instantiate(prefabBullet);
            bullet.transform.position = bulletPos.transform.position;
            Vector3 bulletToMouse = dir - bullet.transform.position;
            float angle = Mathf.Atan2(bulletToMouse.y, bulletToMouse.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().AddForce(bulletToMouse.normalized * bulletSpeed, ForceMode2D.Impulse);
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
