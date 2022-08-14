using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public static partial class Fields
{
    /// <summary>
    /// Disjunção de Conjuto, Modifica field
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static int ExceptFor(this int field, params int[] events)
    {
        int result = field;
        for (var i = 0; i < events.Length; i++)
        {
            var e = events[i];
            result &= ~e;
        }
        return result;
    }
    /// <summary>
    /// Disjunção de Conjuto
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static int ExceptFor(this int field, IEnumerable<int> events) =>
        events.Aggregate(field, (acc, e) => acc & ~e);
    /// <summary>
    /// Uniao de Conjuto
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static int Union(this int field, params int[] events)
    {
        int result = field;
        for (var i = 0; i < events.Length; i++)
        {
            var e = events[i];
            result |= e;
        }
        return result;
    }
    public static int ExceptFor(this int field, int other) => field & ~other;
    public static int Union(this int field, int other) => field | other;
    public static bool HasFlag(this int field, int flag) => (field & flag) == flag;
    /// <summary>
    /// Uniao de Conjuto
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static int Union(this int field, IEnumerable<int> events) =>
        field | events.Aggregate((acc, e) => acc | e);

    /// <summary>
    /// Verifica se ha algum events em field
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static bool AnyOf(this int field, IEnumerable<int> events) =>
            events.Any(e => field.HasFlag(e));
    /// <summary>
    /// Verifica se ha algum events em field
    /// </summary>
    /// <param name="event"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    public static bool AnyOf(this int field, params int[] events) =>
            events.Any(e => field.HasFlag(e));

    public static int Subset(this int field, int events) => field & events;
    public static int Subset(this int field, IEnumerable<int> events) => field & events.Aggregate((acc, e) => acc | e);
    public static int Subset(this int field, params int[] events) => field & events.Aggregate((acc, e) => acc | e);
}
public struct BitField : IEnumerable<bool>, IEquatable<int>
{
    int _bits;
    public BitField(int bits) => _bits = bits;
    public static implicit operator int(BitField field) => field._bits;
    public static implicit operator BitField(int bits) => new(bits);
    public IEnumerable<bool> AsEnumerable()
    {
        for (var i = 0; i < 32; i++)
        {
            yield return _bits.HasFlag(1 << i);
        }
    }
    public override string ToString() => _bits.ToString();
    public IEnumerator<bool> GetEnumerator()
    {
        for (var i = 0; i < 32; i++)
        {
            yield return _bits.HasFlag(1 << i);
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public bool Equals(int other) => _bits.Equals(other);

    public bool this[int index]
    {
        get => _bits.HasFlag(1 << index);
        set => _bits |= (value ? 1 : 0) << index;
    }
}