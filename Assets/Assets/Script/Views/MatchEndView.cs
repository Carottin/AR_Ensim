using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchEndView : MonoBehaviour, IPopulate<MatchStatsModel>
{
    [SerializeField]
    Text m_UnitKilled;
    [SerializeField]
    Text m_LineWon;
    [SerializeField]
    Text m_TotalGain;

    public MatchStatsModel GetData()
    {
        throw new System.NotImplementedException();
    }

    public void Populate(MatchStatsModel data)
    {
        m_UnitKilled.text = data.NbUnitKilled.ToString();
        m_LineWon.text = data.NbLineWon.ToString();
        m_TotalGain.text = data.TotalGain.ToString();
    }

}
