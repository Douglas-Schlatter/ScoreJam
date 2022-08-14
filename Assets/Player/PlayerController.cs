using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vector;
using static Func;
using static State;
using static Event;
public class PlayerController : MonoBehaviour
{
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
        
        var move_state = (Move:     dst_origin(movement),
                          Velocity: dst_origin(rb.velocity));

        @event = move_state switch
        {
            { Move: 0, Velocity: <= 0.3f } => @event.ExceptFor(DoWalk, DoMadDash).Union(EventsFromKeys()),

            { Move: not 0, } => @event.Union(DoWalk).ExceptFor(DoMadDash),

            { Move: 0, } => @event.ExceptFor(DoWalk),
        };
        PlayerStt.Next(@event);
    }
    void FixedUpdate()
    {
        @state = PlayerStt.Current;
        CurrentEvent = Event.ToString(@event);
        CurrentState = State.ToString(@state);
        timer += Time.deltaTime;
        if(timer - lastDashSnap >= 2.0f)
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
            @event = @event.ExceptFor(DoMadDash);
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
    //AKA Em colisão faça
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);

        if (col.CompareTag("Enemy") && (timer - lastHitSnap) > 2.0)
        {
            lastHitSnap = timer;
            life--;
        }
        //Change the spirte of the player to pulsating red for 2 seconds
    }

    int EventsFromKeys()
    {
        int returnEvent = DoNone;
        if (Input.GetKey(KeyCode.R) && !@state.AnyOf(MadDashing, Recharging))
        {
            returnEvent = returnEvent.Union(DoMadDash);
            lastDashSnap = timer;
        }
        else if (!Input.GetKey(KeyCode.R))
        {
            returnEvent = returnEvent.ExceptFor(DoMadDash);
        }
        return returnEvent;
    }
    void MadDash()
    {
        rb.AddForce(DashMod * rb.drag * LookDir.normalized, ForceMode2D.Force);
    }
}
