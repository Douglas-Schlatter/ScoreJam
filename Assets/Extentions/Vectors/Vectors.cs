using System;
using UnityEngine;
public static partial class Vector
{
    public static Vector2 Map(this Vector2 vec2, Func<float, float> func) =>
       new(func(vec2.x), func(vec2.y));
    public static T Map<T>(this Vector2 vec2, Func<Vector2, T> func) =>
        func(vec2);
    public static void Deconstruct(this Vector3 vec, out float x, out float y, out float z) =>
        (x, y, z) = (vec.x, vec.y, vec.z);
    public static void Deconstruct(this Vector2 vec, out float x, out float y) =>
        (x, y) = (vec.x, vec.y);
    public static Func<Vector2, float> Distance(this Vector2 vec2from) =>
        (Vector2 vec2to) => Vector2.Distance(vec2from, vec2to);
    /// <summary>
    /// (f o g)(x) aka f(g(x))  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="f"></param>
    /// <param name="g"></param>
    /// <returns></returns>
    public static void DropForce(this Rigidbody rigidbody)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
    public static void DropForce(this Rigidbody2D rigidbody)
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
    }
}
