using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProgressionManager : Singleton<UnitProgressionManager> {

    public int GetUnitLevel(string name)
    {
        return PlayerPrefs.GetInt(name, 1);
    }

    public void IncrementUnitLevel(string name)
    {
        PlayerPrefs.SetInt(name , GetUnitLevel(name) + 1);
    }

    public int GetPriceToProgress(string name)
    {
        int actualLevel = GetUnitLevel(name);
        return (actualLevel + 1) * 10;
    }
}
