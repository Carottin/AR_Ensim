using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpponentController : MonoBehaviour {

    public static bool CoolDownIsOver = false;
    public int cooldown = 5;
    [SerializeField]
    private GameManager m_gameManager;

    /// <summary>
    /// Used to init the IA
    /// </summary>
    public void Init()
    {
        StartCoroutine(BeforeStart());
    }

    /// <summary>
    /// Used to wait 2 seconds before the ennemy can spawn units
    /// </summary>
    /// <returns></returns>
    public IEnumerator BeforeStart()
    {
        yield return new WaitForSeconds(2);
        CoolDownIsOver = true;
    }

    /// <summary>
    /// Each frame we check if the cooldown is over, then we spawn unit
    /// </summary>
    void Update()
    {
        if (CoolDownIsOver)
        {
            SpawnUnit();
        }
    }

    public void SpawnUnit()
    {
        // Check if the game is in progress
        if (m_gameManager.GameStatus != GameManager.MatchGameStatus.inProgress)
            return;

        CoolDownIsOver = false;
        // Start cooldown for next spawn
        StartCoroutine(WaitUntilCooldown());
        int index = ChooseLine();

        System.Random rnd = new System.Random();

        // Get a random unit in the inventory
        int InventoryIndex = rnd.Next(0, m_gameManager.Inventory.Count);

        // if the unit is too expensive, find another one
        while (m_gameManager.OpponentMoney - m_gameManager.Inventory[InventoryIndex].GetComponent<UnitModel>().Cost < 0)
            InventoryIndex = rnd.Next(0, m_gameManager.Inventory.Count);

        if (m_gameManager.OpponentMoney - m_gameManager.Inventory[InventoryIndex].GetComponent<UnitModel>().Cost < 0)
            return;

        // Insantiate the unit
        var unit = Instantiate(m_gameManager.Inventory[InventoryIndex].gameObject, 
            m_gameManager.MatchLines[index].OpponentSpawnPoint.transform.position,
            m_gameManager.MatchLines[index].OpponentSpawnPoint.transform.rotation,
            m_gameManager.UnitContainer.transform
        );

        // Set all variables in order for the unit, to move and have a target
        unit.GetComponent<UnitController>().SpawnPoint = m_gameManager.MatchLines[index].OpponentSpawnPoint.transform;
        unit.GetComponent<UnitController>().TargetPoint = m_gameManager.MatchLines[index].PlayerSpawnPoint.transform;
        unit.GetComponent<UnitController>().IsPlayer = false;
        unit.GetComponent<UnitController>().LineIndex = index;
        unit.GetComponent<UnitController>().GameManager = this.m_gameManager;

        // add weight to the line
        m_gameManager.MatchLines[index].OpponentWeight += m_gameManager.Inventory[InventoryIndex].Weight;

    }

    /// <summary>
    /// Each units has a weight
    /// Get a line to spawn unit considering player's unit wieght and opponent's unit weight
    /// </summary>
    /// <returns></returns>
    public int ChooseLine()
    {
        List<MatchLineModel> PossibleLines = m_gameManager.MatchLines.FindAll(x => x.Status == MatchLineModel.LineState.inProgress); // get all lines in progress
        // get the line where the player has more unit thant opponent
        int desiredWeight = PossibleLines.Max(x => Math.Abs(x.OpponentWeight - x.PlayerWeight)); 
        int line  = m_gameManager.MatchLines.Find(x => Math.Abs(x.PlayerWeight - x.OpponentWeight) == desiredWeight).index;
        return line;
    }

    /// <summary>
    /// Used to wait before the opponent can spawn another unit
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitUntilCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        CoolDownIsOver = true;
    }

}
