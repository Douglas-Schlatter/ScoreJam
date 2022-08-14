using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BaseEnemyController
{
    //Shooter related
    public float lastShoot = 0;
    public Bullet bullet;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (GameController.iCont.timer - lastShoot > 2.0)
        {
            lastShoot = GameController.iCont.timer;
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.tag = "ABullet";
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
