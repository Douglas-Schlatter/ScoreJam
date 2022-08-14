using UnityEngine;

public interface IDamageSource
{
    void DoDamage(GameObject col, string tag, int damage);
}