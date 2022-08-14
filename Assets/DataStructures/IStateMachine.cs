using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<TState, TEvent, TSelf> where TSelf : IStateMachine<TState, TEvent, TSelf>
{
    TState Current { get; }
    TSelf Next(TEvent @event);
    TState Peek(TEvent @event);
}