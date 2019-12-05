using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPopulate<UnitModel> {

    [SerializeField]
    Text m_name;
    [SerializeField]
    Text m_lifePoints;
    [SerializeField]
    Text m_attackPoints;
    [SerializeField]
    Text m_speed;
    [SerializeField]
    Text m_price;

    [NonSerialized]
    public GameObject Unit;
    [NonSerialized]
    public GameObject SpawnPoint;

    [NonSerialized]
    public GameObject Table;

    private UnitModel m_model;

    public PlayerController Player;

    public ProgressionView ProgressionView;

    public UnitModel GetData()
    {
        return m_model;
    }



    public void Populate(UnitModel data)
    {
        m_model = data;

        if (m_name)
            m_name.text = data.Name;
        if (m_lifePoints)
            m_name.text = data.LifePoints.ToString();
        if (m_attackPoints)
            m_name.text = data.AttackPoints.ToString();
        if (m_speed)
            m_name.text = data.Speed.ToString();
        if (m_price)
            m_price.text = data.Cost.ToString();
    }

    public void SpawnUnit()
    {
        if (MenuManager.GlobalState==MenuManager.GlobaState.inMatch)
            Player.SpawnUnit(Unit, m_model);
        else if(MenuManager.GlobalState == MenuManager.GlobaState.inProgressionScreen)
        {
            ProgressionView.Populate(m_model);
            ProgressionView.SetUnitModel(Unit);
        }
    }


}
