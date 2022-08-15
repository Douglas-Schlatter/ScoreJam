using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BaseEnemyController : MonoBehaviour
{
    public int health = 2;

    public GameObject deathEffect;
    public float lastHit = 0;

    //Path related
    public AIPath pf;
    public AIDestinationSetter ds;
  


    private void Awake()
    {
        //ds.target = GameController.iCont.player.transform;
    }
    private void Start()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        if (GameController.iCont.timer - lastHit > 0.1)
        {
            //pf.canMove = true;
            // pf.canSearch = false;
            //ds.StartCoroutine();
            //ds.target = GameController.iCont.player.transform;
            pf.enabled = true;
        }
    }
    public void TakeDamage(int damage)
    {
  
        lastHit = GameController.iCont.timer;
        health -= damage;
        //pf.canMove = false;

        //ds.target = null;
        //pf.canSearch = false;
        pf.enabled = false;

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        GameController.iCont.sEnemies.Remove(this.gameObject);
        GameController.iCont.GiveScore(1);
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }



    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */
    // Update is called once per frame
    protected virtual void Update()
    {
        if (pf.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            //transform.localRotation = new Quaternion(0, 0, -90,0);
        }
        else if (pf.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            //transform.localRotation = new Quaternion(0, 0, 90, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Slash"))
        {
            health -= 2 ;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    

}
