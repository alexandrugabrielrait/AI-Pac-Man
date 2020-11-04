using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IComparable<Move>
{
    public int direction;
    public float value;

    public Move(int direction, float value)
    {
        this.direction = direction;
        this.value = value;
    }

    public int CompareTo(Move other)
    {
        return Mathf.RoundToInt(other.value - value);
    }
}
