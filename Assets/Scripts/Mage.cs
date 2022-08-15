using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;
using static Event;


public class Mage : BaseEnemyController
{
    public float lastShoot = 0;
    public int lazerDmg = 1;
    public Transform firePoint;
    public LineRenderer lineRender;

        IEnumerator ShootLazer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up);
        //RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, GameController.iCont.player.transform.position);

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.transform.CompareTag("Player"))
            {
                
                    PlayerController pl = hitInfo.transform.GetComponent<PlayerController>();
                    pl.TakeDamage(1, this.tag);
            }



            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, firePoint.position + firePoint.up * 100);
            //lineRender.SetPosition(1, firePoint.position + GameController.iCont.player.transform.position * 100);

        }
        lineRender.enabled = true;

        //wait one frame
        yield return new WaitForSeconds(0.1f);

        lineRender.enabled = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameController.iCont.timer - lastShoot > 2.5)
        {
            lastShoot = GameController.iCont.timer;
            StartCoroutine(ShootLazer());
        }
    }
    // Update is called once per fra
}
