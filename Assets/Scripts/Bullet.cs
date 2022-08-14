using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public int damage = 1;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        Destroy(gameObject);

        if (this.tag != "ABullet")
        {
            if (col.CompareTag("Enemy"))
            {
                BaseEnemyController enemyCtr = col.GetComponent<BaseEnemyController>();
                enemyCtr.TakeDamage(damage);
            }
        }

        


    }
}
