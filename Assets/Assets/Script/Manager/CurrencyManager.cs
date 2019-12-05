using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager> {

    public string CurrencyKey = "Currency";
    int PlayerMoney = 0;

    public void Intialize()
    {
        PlayerMoney = GetPlayerCurrency();
    }

    /// <summary>
    /// Return current player currency using playerprefs
    /// </summary>
    /// <returns></returns>
    public int GetPlayerCurrency()
    {
        return PlayerPrefs.GetInt(CurrencyKey, 0);
    }

    /// <summary>
    /// Add currency to the player using playerprefs
    /// </summary>
    /// <param name="amount"></param>
    public void AddPlayerCurrency(int amount)
    {
        PlayerPrefs.SetInt(CurrencyKey, GetPlayerCurrency() + amount);
        PlayerMoney += amount;
    }

}
