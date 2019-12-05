using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel : MonoBehaviour {

    public string Name;
    public float LifePoints;
    public float Speed;
    public float AttackPoints;
    public int Cost;
    public int Weight;
    public int MoneyEarn;

    public float GetRealLevel(bool isPlayer=true)
    {
        float level = UnitProgressionManager.Instance.GetUnitLevel(Name);
        return isPlayer ? 1 + (level / 10) : 1;
    }
}
