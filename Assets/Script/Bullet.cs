using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;

    public void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Wait(10f));
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPoolManager.ReturnToPool("Bullet", gameObject);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ground") || coll.gameObject.CompareTag("Enemy"))
        {
            ObjectPoolManager.ReturnToPool("Bullet", gameObject);
        }
    }
}
