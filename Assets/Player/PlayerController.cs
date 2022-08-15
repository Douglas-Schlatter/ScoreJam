using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Vector;
using static Func;
using static State;
using static Event;
public class PlayerController : MonoBehaviour, IDamageSource
{
    float _munch = 1;

    public event Action<int> OnStateChanged;

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
        @state = PlayerStt.Current;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        static float dst_origin(Vector2 vec2) => Compose(Mathf.Abs, Vector.Distance(vec2))(Vector2.zero);

        if(@state.HasFlag(Walking) && !state.HasFlag(MadDashing))
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
        }
        if (dst_origin(movement) is not 0)
        {
            PlayerStt.Next(DoWalk);
        }
        if (dst_origin(rb.velocity) is 0)
        {
            PlayerStt.Next(DoNone);
            OnStateChanged?.Invoke(@state);
        }
        if (timer - lastDashSnap > 2.0f)
        {
            PlayerStt.Next(RechargeDash);
        }
        EventsFromKeys();
    }
    void FixedUpdate()
    {
        @state = PlayerStt.Current;
        CurrentEvent = Event.ToString(@event);
        CurrentState = State.ToString(@state);
        timer += Time.deltaTime;

        float angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    //AKA Em colisão faça
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (!state.HasFlag(MadDashing))
        {
            if (col.CompareTag("Enemy") || col.CompareTag("ABullet") && (timer - lastHitSnap) > 2.0)
            {
                @event = @event.Union(GetHurt);
                lastHitSnap = timer;
                life--;
                PlayerStt.Next(GetHurt);
                OnStateChanged?.Invoke(PlayerStt.Current);
            }
        }
        else if (@state.HasFlag(MadDashing))
        {
            lastHitSnap = timer;
            DoDamage(col.gameObject, "Enemy", DashDamage);
            gameObject.transform.localScale = MunchScale;
            PlayerStt.Next(DoNone);
        }
        //Change the spirte of the player to pulsating red for 2 seconds
    }
    void EventsFromKeys()
    {
        if (Input.GetKey(KeyCode.Space) && !@state.HasFlag(Recharging))
        {
            lastDashSnap = timer;
            MadDash();
            PlayerStt.Next(DoMadDash);
            OnStateChanged?.Invoke(PlayerStt.Current);
        }
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
}
