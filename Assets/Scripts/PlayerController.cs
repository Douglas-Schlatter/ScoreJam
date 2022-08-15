using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vector;
using static Func;
using static State;
using static Event;
public class PlayerController : MonoBehaviour, IDamageSource
{
    float _munch = 1;

    Vector3 MunchScale
    {
        get
        {
            var vec5 = new Vector3(4, 4, 4);
            _munch += 0.01f;
            return Vector3.Min(vec5, transform.localScale * _munch);
        }
    }
    //State
    public PlayerState PlayerStt { get; } = new();

    //Status Effects
    public float moveSpeed = 5f;
    public int life = 3;
    //Time Related
    public float timer = 0.0f;
    public float lastHitSnap =-2.0f;
    public float lastDashSnap = 0.0f;

    //public Rigidbody2D rb;
    public Rigidbody2D rb;
    public Camera cam;

    //Movement related
    Vector2 movement;
    Vector2 mousePos;
    Vector2 LookDir => mousePos - rb.position;
    public bool canDash = true;
    public bool isDash = false;

    public int DashDamage = 2;

    int @event = DoNone;
    int @state;

    [SerializeField]
    string CurrentEvent;
    [SerializeField]
    string CurrentState;

    public float DashMod = 500f;

    //Weapon Related
    public GameObject[] wps;
    float targetAngle;
    public int curWpn;
    public ShooterController sc;
    public bool isSlash = false;

    //Sprite related
    public SpriteRenderer spriR;
    public Sprite BaseSprite;
    public Sprite HurtSprite;
    public Sprite MadDashSprite;


    private void Awake()
    {
        rb = rb is null ? GetComponent<Rigidbody2D>() : rb;
        cam = cam is null ? Camera.main : cam;
        life = life <0 ? 3 : life;
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            Debug.Log("DASH");
            canDash = false;
            isDash = true;
            lastDashSnap = GameController.iCont.timer;
            spriR.sprite = MadDashSprite;
            //MadDash();
        }
        if (isDash && (GameController.iCont.timer - lastDashSnap) < 0.2)
        {
            MadDash();
        }
        else if (isDash && (GameController.iCont.timer - lastDashSnap) > 0.2)
        {
            isDash = false;
            spriR.sprite = BaseSprite;
        }

        if(!isDash && (GameController.iCont.timer - lastDashSnap) > 2.0)
        {
            canDash = true;
        }


    }
    void FixedUpdate()
    {

        timer += Time.deltaTime;
       
        float angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (!isDash && (GameController.iCont.timer - lastHitSnap) > 1.0)
        { 
        
            spriR.sprite = BaseSprite;
        }
        //girar aqui a weapon
        //Debug.Log("Rotation: " + this.transform.localRotation.eulerAngles.z);
        targetAngle = this.transform.localRotation.eulerAngles.z;
        if (0<= targetAngle && targetAngle <= 180f)   
        {
            if ( wps[curWpn].transform.localScale.y<0)
            {
                wps[curWpn].transform.localScale = new Vector3(wps[curWpn].transform.localScale.x, -wps[curWpn].transform.localScale.y, wps[curWpn].transform.localScale.z);
            }
        }
        else if ( 180 < targetAngle && targetAngle <= 360f)
        {
            if (0<wps[curWpn].transform.localScale.y)
            {
                wps[curWpn].transform.localScale = new Vector3(wps[curWpn].transform.localScale.x, -wps[curWpn].transform.localScale.y, wps[curWpn].transform.localScale.z);
            }
            //wps[0].transform.localScale = new Vector3(wps[0].transform.localScale.x, wps[0].transform.localScale.y, wps[0].transform.localScale.z);
        }


    }


    //AKA Em colis�o fa�a
    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.name);
        if (!isDash && !isSlash)
        {

            if (col.CompareTag("Enemy") || col.CompareTag("ABullet") || col.CompareTag("A") || col.CompareTag("K") || col.CompareTag("M") && (timer - lastHitSnap) > 2.0)
            {
               // Debug.Log("DANO");
                lastHitSnap = timer;
                spriR.sprite = HurtSprite;
                life--;
            }
        }
        else if (isDash)
        {
            lastHitSnap = timer;
            DoDamage(col.gameObject, "Enemy", DashDamage);

            if (col.CompareTag("A"))
            {
                DoDamage(col.gameObject, "A", DashDamage);
                curWpn = 2;
                wps[2].GetComponent<SpriteRenderer>().enabled = true;
                wps[0].GetComponent<SpriteRenderer>().enabled = false;
                wps[1].GetComponent<SpriteRenderer>().enabled = false;
                sc.bullet = true;
                sc.lazer = false;
                sc.sword = false;
            }
            else if (col.CompareTag("M"))
            {
                DoDamage(col.gameObject, "M", DashDamage);
                curWpn = 1;
                wps[1].GetComponent<SpriteRenderer>().enabled = true;
                wps[2].GetComponent<SpriteRenderer>().enabled = false;
                wps[0].GetComponent<SpriteRenderer>().enabled = false;
                sc.bullet = false;
                sc.lazer = true;
                sc.sword = false;
            }
            else if (col.CompareTag("K"))
            {
                DoDamage(col.gameObject, "K", DashDamage);
                curWpn = 0;
                wps[0].GetComponent<SpriteRenderer>().enabled = true;
                wps[1].GetComponent<SpriteRenderer>().enabled = false;
                wps[2].GetComponent<SpriteRenderer>().enabled = false;
                sc.bullet = false;
                sc.lazer = false;
                sc.sword = true;
            }
            
            
            
            gameObject.transform.localScale = MunchScale;
            
        }
        //Change the spirte of the player to pulsating red for 2 seconds
    }
    int EventsFromKeys()
    {
        int returnEvent = DoNone;
        if (Input.GetKey(KeyCode.Space) && !@state.HasFlag(Recharging))
        {
            returnEvent = returnEvent.Union(DoMadDash);
            lastDashSnap = timer;
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            returnEvent = returnEvent.ExceptFor(DoMadDash);
        }
        return returnEvent;
    }
    void MadDash()
    {
        rb.AddForce(DashMod * LookDir.normalized, ForceMode2D.Force);
    }
    public void DoDamage(GameObject col, string tag, int damage)
    {
        if (col.CompareTag(tag))
        {
            BaseEnemyController enemyCtr = col.GetComponent<BaseEnemyController>();
            enemyCtr.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage, string tag)
    {
        if (!isDash && !isSlash)
        {
            Debug.Log("Pode tomar dano");

            if (tag == "Enemy" || tag ==  "A" || tag ==  "M" || tag ==  "K" && (timer - lastHitSnap) > 2.0)
            {
                lastHitSnap = timer;
                life--;
            }
        }
    }
}
