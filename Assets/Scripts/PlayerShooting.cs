using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = null;
    [SerializeField]
    private Bullet bulletObject = null;
    [SerializeField]
    float timeBetweenShots = 0.1f;
    [SerializeField]
    AudioClip[] shootSFX = null;
    private Queue<Bullet> BulletObjects { get; set; } = new Queue<Bullet>();
    Bullet bullet = null;
    float timeSinceLastShot = 0;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(timeSinceLastShot >= timeBetweenShots)
            {
                ShootBullet();
            }
        }
        timeSinceLastShot += Time.deltaTime;
    }

    private Vector3 GetMousePos()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void ShootBullet()
    {
        if (bulletObject == null || shootSFX == null) return;
        bullet = ReturnBullet();
        ResetBullet();
        AudioSource.PlayClipAtPoint(shootSFX[Random.Range(0, shootSFX.Length-1)], mainCamera.transform.position);
        timeSinceLastShot = 0;
    }

    private void ResetBullet()
    {
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        bullet.BulletData = new BulletData(GetMousePos(), this);
    }

    private Bullet ReturnBullet()
    {
        if(BulletObjects.Count == 0)
        {
            BulletObjects.Enqueue(InstantiateBullet());
        }
        return BulletObjects.Dequeue();
    }

    private Bullet InstantiateBullet()
    {
        return Instantiate(bulletObject, transform.position, Quaternion.identity);
    }

    public void EnqueueBullet(Bullet bullet)
    {
        BulletObjects.Enqueue(bullet);
    }
}
