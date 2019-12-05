﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static readonly T instance = new T();

    static Singleton()
    {
    }

    protected Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
}
