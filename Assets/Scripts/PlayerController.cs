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
            var vec5 = new Vector3(5, 5, 5);
            _munch += 0.01f;
            return Vector3.Min(vec5, transform.localScale * _munch);
        }
    }
    //State
    public PlayerState PlayerStt { get; } = new();

    //Status Effects
    public float moveSpeed = 5f;
    public float life = 3f;

    //Time Related
    public float timer = 0.0f;
    public float lastHitSnap = 0.0f;
    public float lastDashSnap = 0.0f;

    //public Rigidbody2D rb;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 LookDir => mousePos - rb.position;

    public int DashDamage = 2;

    int @event = DoNone;
    int @state;

    [SerializeField]
    string CurrentEvent;
    [SerializeField]
    string CurrentState;

    public float DashMod = 125f;


    private void Awake()
    {
        rb = rb is null ? GetComponent<Rigidbody2D>() : rb;
        cam = cam is null ? Camera.main : cam;
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        static float dst_origin(Vector2 vec2) => Compose(Mathf.Abs, Vector.Distance(vec2))(Vector2.zero);

        var move_state = (Move: dst_origin(movement),
                          Velocity: dst_origin(rb.velocity));

        @event = move_state switch
        {
            { Move: 0, Velocity: <= 0.1f } => @event.ExceptFor(DoWalk | DoMadDash).Union(EventsFromKeys()),

            { Move: not 0 } => @event.Union(DoWalk).ExceptFor(DoMadDash),
            { Move: 0 } => @event.ExceptFor(DoWalk).Union(EventsFromKeys()),
        };
        PlayerStt.Next(@event);
    }
    void FixedUpdate()
    {
        @state = PlayerStt.Current;
        CurrentEvent = Event.ToString(@event);
        CurrentState = State.ToString(@state);
        timer += Time.deltaTime;
        if (timer - lastDashSnap >= 2.0f)
        {
            @event = @event.Union(RechargeDash);
        }
        else
        {
            @event = @event.ExceptFor(RechargeDash);
        }
        if (@state.HasFlag(MadDashing) && !@state.HasFlag(Recharging))
        {
            MadDash();
        }
        else if (@state.HasFlag(Walking))
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
            rb.DropForce();
        }
        float angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        PlayerStt.Next(@event);
    }


    //AKA Em colis�o fa�a
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (!state.HasFlag(MadDashing))
        {

            if (col.CompareTag("Enemy") || col.CompareTag("ABullet") && (timer - lastHitSnap) > 2.0)
        {
                lastHitSnap = timer;
                GameController.TakeDamage();;
            }
        }
        else if (@state.HasFlag(MadDashing))
        {
            lastHitSnap = timer;
            DoDamage(col.gameObject, "Enemy", DashDamage);

            gameObject.transform.localScale = MunchScale;
            PlayerStt.Next(@event.Union(DoWalk).ExceptFor(DoMadDash));
            
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
        rb.AddForce(DashMod * Mathf.Max(1, rb.drag) * LookDir.normalized, ForceMode2D.Force);
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
        if (!state.HasFlag(MadDashing))
        {

            if (tag == "Enemy" || tag == "ABullet" && (timer - lastHitSnap) > 2.0)
            {
                lastHitSnap = timer;
                life--;
            }
        }
    }
}
