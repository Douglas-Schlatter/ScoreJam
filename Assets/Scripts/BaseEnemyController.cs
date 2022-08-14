using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    public int health = 2;

    public GameObject deathEffect;
    public float lastHit = 0;
    public Pathfinding.AIPath pf;
    public Pathfinding.AIDestinationSetter ds;



    private void Awake()
    {
        ds.target = GameController.iCont.player.transform;
    }

    private void FixedUpdate()
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
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }



    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
