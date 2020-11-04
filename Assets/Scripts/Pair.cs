﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair : IComparable<Pair>
{
    public int first;
    public int second;

    public Pair(int first, int second)
    {
        this.first = first;
        this.second = second;
    }

    public int CompareTo(Pair other)
    {
       return second - other.second;
    }
}
