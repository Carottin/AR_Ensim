using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contain all method to increment th player money at the end of a match
/// </summary>
public class MatchStatsManager : Singleton<MatchStatsManager> {

    public MatchStatsModel MatchStats = new MatchStatsModel();

    public int MoneyPerKill = 3;
    public int MoneyPerLine = 10;
    public int MoneyPerWin = 20;

    public void ResetStats()
    {
        MatchStats = new MatchStatsModel()
        {
            NbLineWon = 0,
            NbUnitKilled = 0
        };
    }

    public void IncrementUnitKilled()
    {
        MatchStats.NbUnitKilled++;
    }

    public void IncrementLineWon()
    {
        MatchStats.NbLineWon++;
    }

    public int ComputeMatchGain()
    {
        int TotalMoney = MatchStats.NbUnitKilled * MoneyPerLine + MatchStats.NbLineWon * MoneyPerLine + (MatchStats.NbLineWon>=2 ? MoneyPerWin : 0);
        MatchStats.TotalGain = TotalMoney;
        return TotalMoney;
    }
}
