using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopulate<T>
{
    void Populate(T data);

    T GetData();

}
