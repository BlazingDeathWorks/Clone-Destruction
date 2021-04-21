using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 1f;
    public BulletData BulletData { private get; set; } = null;

    private void Update()
    {
        if (BulletData == null) return;
        if(new Vector2(transform.position.x, transform.position.y) == BulletData.targetPos)
        {
            DestroyBullet();
        }
    }

    private void FixedUpdate()
    {
        if (BulletData == null) return;
        transform.position = Vector2.MoveTowards(transform.position, BulletData.targetPos, bulletSpeed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        BulletData.playerShooting.EnqueueBullet(this);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }
}
