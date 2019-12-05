using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProgressionController : MonoBehaviour {
    [SerializeField]
    private ProgressionView m_progressionView;
    [SerializeField]
    private GameManager m_gameManager;

    [Header("Inventory")]
    [SerializeField]
    private CardView m_card;
    [SerializeField]
    private GameObject m_cardsContainer;

    public void Initialize()
    {
        ClearInventory();
        var t = m_gameManager.Inventory;

        // add a card for each unit in inventory
        foreach (UnitModel um in m_gameManager.Inventory)
        {
            var card = Instantiate(m_card, m_cardsContainer.transform);
            card.Populate(um);
            card.Unit = um.gameObject;
            card.ProgressionView = m_progressionView;
        }

        // populate the progression interface with the first unit
        m_progressionView.Populate(m_gameManager.Inventory[0]);
        m_progressionView.SetUnitModel(m_gameManager.Inventory[0].gameObject);
        m_progressionView.ActiveUnitName = m_gameManager.Inventory[0].Name;
    }

    /// <summary>
    /// Used to make a unit progress
    /// </summary>
    public void IncrementUnitProgression()
    {
        string name = m_progressionView.ActiveUnitName;
        if (UnitProgressionManager.Instance.GetPriceToProgress(name) <= CurrencyManager.Instance.GetPlayerCurrency())
        {
            UnitProgressionManager.Instance.IncrementUnitLevel(name);
            CurrencyManager.Instance.AddPlayerCurrency(-UnitProgressionManager.Instance.GetPriceToProgress(name));
            m_progressionView.Populate(m_gameManager.Inventory.Find(x=>x.Name == name));
            m_progressionView.UpdateDisplay(name);
        }
    }


    /// <summary>
    /// Used to destroy all cards
    /// </summary>
    public void ClearInventory()
    {
        foreach (Transform child in m_cardsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
