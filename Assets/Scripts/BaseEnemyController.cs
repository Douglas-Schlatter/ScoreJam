using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    public int health = 2;

    public GameObject deathEffect;
    public float lastHit = 0;
    public  GameObject a;
    public Pathfinding.AIDestinationSetter pf;



    private void Awake()
    {
        pf = a.GetComponentInChildren<Pathfinding.AIDestinationSetter>();
        pf.target = GameController.iCont.player.transform;
    }

    public void TakeDamage(int damage)
    {
  
        lastHit = GameController.iCont.timer;
        health -= damage;
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
