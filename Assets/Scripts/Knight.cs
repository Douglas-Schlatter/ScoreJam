using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Func;
public class Knight : BaseEnemyController
{
    public SpriteRenderer ef;
    public Collider2D col;
    public float lastSlice = 0;
    public float duration = 0;
    float dst_origin(Vector2 vec2) => Compose(Mathf.Abs, Vector.Distance(vec2))(this.transform.position);
     void Update()
    {
        base.Update();
        if (dst_origin(GameController.iCont.player.transform.position) < 2.0 && GameController.iCont.timer - lastSlice > 2.0)
        {
            Debug.Log("SLICE!");
            lastSlice = GameController.iCont.timer;

            ef.enabled = true;
            col.enabled = true;
        }
    }
    void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameController.iCont.timer - duration > 1.0)
        {
            duration = 0;
            ef.enabled = false;
            col.enabled = false;
        }
        else
        {
            duration = GameController.iCont.timer;
        }
    }

}
