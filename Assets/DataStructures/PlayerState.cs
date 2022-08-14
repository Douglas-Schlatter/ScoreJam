using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static State;
using static Event;
public static class State
{
    public const int None = 0 << 0;
    public const int Idle = 1 << 0;
    public const int Walking = 1 << 1;
    public const int MadDashing = 1 << 2;
    public const int Recharging = 1 << 3;
    public const int Hurt = 1 << 4;
    public static string ToString(int state)
    {
        string str = "";
        if (state.HasFlag(None))
            str += nameof(None) + ",";
        if (state.HasFlag(Idle))
            str += nameof(Idle) + ",";
        if (state.HasFlag(Walking))
            str += nameof(Walking) + ",";
        if (state.HasFlag(MadDashing))
            str += nameof(MadDashing) + ",";
        if (state.HasFlag(Recharging))
            str += nameof(Recharging) + ",";
        if (state.HasFlag(Hurt))
            str += nameof(Hurt) + ",";
        return str[..^1];
    }
}
public static class Event
{
    public const int DoNone = 0 << 0;
    public const int DoWalk = 1 << 0;
    public const int DoMadDash = 1 << 1;
    public const int RechargeDash = 1 << 2;
    public const int GetHurt = 1 << 3;

    public static string ToString(int @event)
    {
        string str = "";
        if (@event.HasFlag(DoNone))
            str += nameof(DoNone) + ",";
        if (@event.HasFlag(DoWalk))
            str += nameof(DoWalk) + ",";
        if (@event.HasFlag(DoMadDash))
            str += nameof(DoMadDash)+ ",";
        if (@event.HasFlag(RechargeDash))
            str += nameof(RechargeDash)+ ",";
        if (@event.HasFlag(GetHurt))
            str += nameof(GetHurt)+ ",";
        
        return str[..^1];
    }
}
public class PlayerState : IStateMachine<int, int, PlayerState>
{
    public int Current { get; private set; }
    public PlayerState Next(int @event)
    {
        Current = Peek(@event);
        return this;
    }
    public int Peek(int @event) => StateMachineMap(Current, @event);
    int StateMachineMap(int @state, int @event)
    {
        int moveEvents = @event.Subset(DoMadDash | DoWalk);
        var result = (@state, @event: moveEvents) switch
        {
            { @state: <= Recharging, @event: DoMadDash } => @state.ExceptFor(Walking).Union(MadDashing|Recharging),
            { @event: DoNone } => @state.ExceptFor(MadDashing | Walking),
            { @event: DoWalk } => @state.Union(Walking).ExceptFor(MadDashing),
            { /* default */  } => @state,
        };
        result = @event.ExceptFor(moveEvents) switch
        {
            RechargeDash => result.ExceptFor(Recharging, Hurt),
            GetHurt => result.Union(Hurt),
            _ => result,
        };
        return result;
    }
}