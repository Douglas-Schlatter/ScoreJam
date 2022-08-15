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

    //swing related
    public SpriteRenderer ef;
    public Collider2D col;
    public Collider2D col2;
    public float duration = 0;
    public float lastSlice = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (bullet)
                ShootBullet();
            else if (lazer)
                StartCoroutine(ShootLazer());
            else if (sword && (GameController.iCont.timer - lastSlice) > 0.6)
            {
                Swing();
            }
        }
        if (GameController.iCont.timer - duration > 0.4)
        {
            GameController.iCont.player.GetComponent<PlayerController>().isSlash = false;
            duration = 0;
            ef.enabled = false;
            col.enabled = false;
            //col2.enabled = false; // phisical colission
        }
        if (GameController.iCont.timer - lastSlice >= 0.6)
        {
            lastSlice = 0;
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
        yield return new WaitForSeconds(0.05f);

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
        duration = GameController.iCont.timer;
        lastSlice = GameController.iCont.timer;
        GameController.iCont.player.GetComponent<PlayerController>().isSlash = true;
        ef.enabled = true;
        col.enabled = true;
       // col2.enabled = true;
    }
}
