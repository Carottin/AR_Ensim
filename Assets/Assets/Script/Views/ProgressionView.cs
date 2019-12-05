using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionView : MonoBehaviour , IPopulate<UnitModel>{

    [SerializeField]
    Text m_UnitName;
    [SerializeField]
    Text m_HP;
    [SerializeField]
    Text m_Speed;
    [SerializeField]
    Text m_Attack;
    [SerializeField]
    Text m_PlayerMoney;
    [SerializeField]
    Text m_UnitLevel;
    [SerializeField]
    Text m_UnitCost;
    [SerializeField]
    Text m_PriceToEvolve;

    [SerializeField]
    GameObject m_UnitContainer;

    [NonSerialized]
    public string ActiveUnitName = "";
    public UnitModel GetData()
    {
        throw new System.NotImplementedException();
    }

    public void Populate(UnitModel data)
    {
        m_UnitName.text = data.Name;
        m_HP.text = (data.LifePoints * data.GetRealLevel()).ToString();
        m_Speed.text = (data.Speed * data.GetRealLevel()).ToString();
        m_Attack.text = (data.AttackPoints * data.GetRealLevel()).ToString();
        m_PlayerMoney.text = CurrencyManager.Instance.GetPlayerCurrency().ToString();
        m_PriceToEvolve.text = UnitProgressionManager.Instance.GetPriceToProgress(data.Name).ToString();
        m_UnitLevel.text = UnitProgressionManager.Instance.GetUnitLevel(data.Name).ToString();
        m_UnitCost.text = data.Cost.ToString();
    }


    public void SetUnitModel(GameObject Unit)
    {
        foreach (Transform child in m_UnitContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        var unit = Instantiate(Unit, m_UnitContainer.transform.position, m_UnitContainer.transform.rotation, m_UnitContainer.transform);
        unit.GetComponent<UnitController>().IsPlayer = true;
        unit.GetComponent<UnitMaterialSwitcher>().SetMaterial();
    }

    public void IncrementUnitProgression()
    {
        string name = m_UnitName.text.ToString();
        if (UnitProgressionManager.Instance.GetPriceToProgress(name) <= CurrencyManager.Instance.GetPlayerCurrency())
        {
            UnitProgressionManager.Instance.IncrementUnitLevel(name);
            CurrencyManager.Instance.AddPlayerCurrency(-UnitProgressionManager.Instance.GetPriceToProgress(name));
            m_PlayerMoney.text = CurrencyManager.Instance.GetPlayerCurrency().ToString();
            m_UnitLevel.text = UnitProgressionManager.Instance.GetUnitLevel(name).ToString();
            m_PriceToEvolve.text = UnitProgressionManager.Instance.GetPriceToProgress(name).ToString();
        }
    }

    public void UpdateDisplay(string UnitName)
    {
        m_PlayerMoney.text = CurrencyManager.Instance.GetPlayerCurrency().ToString();
        m_UnitLevel.text = UnitProgressionManager.Instance.GetUnitLevel(name).ToString();
        m_PriceToEvolve.text = UnitProgressionManager.Instance.GetPriceToProgress(name).ToString();
    }

}
