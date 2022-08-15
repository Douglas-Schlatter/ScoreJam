using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static State;
using static Event;
public class PlayerSprites : MonoBehaviour, IEnumerable<Sprite>
{
    public SpriteRenderer SpriteRenderer;
    public PlayerController PlayerCtrl;
    public Sprite BaseSprite;
    public Sprite HurtSprite;
    public Sprite MadDashSprite;
    public Sprite HurtDashSprite;
    private void Awake()
    {
        PlayerCtrl = GetComponent<PlayerController>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BaseSprite = SpriteRenderer.sprite;
        PlayerCtrl.OnStateChanged += SubscribrChangeSprite;
    }
    public void SubscribrChangeSprite(int stt)
    {
        if(stt.HasFlag(Hurt|MadDashing))
        {
            SpriteRenderer.sprite = HurtDashSprite;
        }
        else if (stt.HasFlag(Hurt))
        {
            SpriteRenderer.sprite = HurtSprite;
        }
        else if (stt.HasFlag(MadDashing))
        {
            SpriteRenderer.sprite = MadDashSprite;
        }
        else if (stt.HasFlag(Walking))
        {
            SpriteRenderer.sprite = BaseSprite;
        }
    }
    public IEnumerable<Sprite> AsEnumerable()
    {
        yield return HurtSprite;
        yield return MadDashSprite;
    }
    public IEnumerator<Sprite> GetEnumerator() => AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Sprite this[int i]
    {
        get => AsEnumerable().ElementAt(i);
    }
}
