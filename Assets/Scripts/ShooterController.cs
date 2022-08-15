using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{

    public bool bullet = false;
    public bool lazer = false;
    public bool sword = false;

    public int lazerDmg = 1;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public LineRenderer lineRender;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (bullet)
                ShootBullet();
            else if (lazer)
                StartCoroutine(ShootLazer());
            else if (sword)
            {
                Swing();
            }
        }
    }

    IEnumerator ShootLazer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up);
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.transform.CompareTag("Enemy")|| hitInfo.transform.CompareTag("A") || hitInfo.transform.CompareTag("M") || hitInfo.transform.CompareTag("K"))
            {
                BaseEnemyController enemyCtr = hitInfo.transform.GetComponent<BaseEnemyController>();
                enemyCtr.TakeDamage(lazerDmg);
            }



            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, firePoint.position + firePoint.up *100);
        }
        lineRender.enabled = true;

        //wait one frame
        yield return new WaitForSeconds(0.02f);

        lineRender.enabled = false;

    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb =  bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void Swing() 
    {
    
    }
}
