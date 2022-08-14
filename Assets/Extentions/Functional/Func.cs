using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class Func
{
    public static (R1, R2) Map<T1, T2, R1, R2>(this (T1, T2) tuple, Func<T1, R1> f, Func<T2, R2> g) =>
        (f(tuple.Item1), g(tuple.Item2));
    public static (R, R) Map<T, R>(this (T, T) tuple, Func<T, R> mapper) =>
        (mapper(tuple.Item1), mapper(tuple.Item2));

    public static Func<T, R> Compose<T, T1, R>(Func<T1, R> f, Func<T, T1> g) => x => f(g(x));
}
