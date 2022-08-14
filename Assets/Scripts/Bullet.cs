using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageSource
{
    public GameObject hitEffect;
    public int damage = 1;

    public void DoDamage(GameObject col, string tag, int damage)
    {
        if (col.CompareTag(tag))
        {
            BaseEnemyController enemyCtr = col.GetComponent<BaseEnemyController>();
            enemyCtr.TakeDamage(damage);
        }
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);
        if (tag != "ABullet")
        {
            DoDamage(col.gameObject, "Enemy", damage);      
        }


        
    }
}