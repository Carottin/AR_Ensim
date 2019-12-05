using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Lang;

public class GameManager : MonoBehaviour {

    public enum MatchGameStatus
    {
        inProgress,
        playerWon,
        opponentWon
    }

    [Header("Global")]
    [NonSerialized]
    public List<MatchLineModel> MatchLines = new List<MatchLineModel>();
    [SerializeField]
    public GameObject UnitContainer;
    [NonSerialized]
    public MatchGameStatus GameStatus = MatchGameStatus.inProgress;
    [SerializeField]
    private OpponentController m_opponent;
    [SerializeField]
    private PlayerController m_player;
    [SerializeField]
    private MatchEndView m_matchEndView;

    private bool m_matchHasEnded = false;

    [Header("SpawnPoints")]
    [SerializeField]
    private List<Transform> m_PlayerSpawnPoints;
    [SerializeField]
    private List<Transform> m_OpponentSpawnPoints;

    [Header("Inventory")]
    [SerializeField]
    public List<UnitModel> Inventory;

    [NonSerialized]
    public int PlayerMoney = 50;
    [NonSerialized]
    public int OpponentMoney = 50;

    [NonSerialized]
    public int LineFinalMoney = 30;

    [Header("EndObjects")]
    [SerializeField]
    private List<UnitController> m_EndObjects;

    /// <summary>
    /// Used to clear everyting before the game start
    /// </summary>
    public void Init()
    {
        MatchStatsManager.Instance.ResetStats();
        m_matchHasEnded = false;
        DestroyAllUnits();
        PlayerMoney = 40;
        OpponentMoney = 40;
        foreach (Transform t in m_PlayerSpawnPoints)
            t.gameObject.SetActive(true);
        foreach (Transform t in m_OpponentSpawnPoints)
            t.gameObject.SetActive(true);
        foreach (UnitController uc in m_EndObjects)
        {
            uc.gameObject.SetActive(true);
            uc.life = 50;
        }
        GameStatus = MatchGameStatus.inProgress;
        InitLines();
        m_opponent.Init();
        m_player.Init();
        StartCoroutine(IncrementMoney());
    }

    public void Quit()
    {
        DestroyAllUnits();
    }

    /// <summary>
    /// Each frames check if the game has ended
    /// </summary>
    private void Update()
    {
        if(!m_matchHasEnded)
            CheckForGameEnd();
    }

    /// <summary>
    /// Used to initialize all game lines
    /// </summary>
    public void InitLines()
    {
        MatchLines.Clear();
        for (int i = 0; i < 3; i++)
        {
            MatchLines.Add(new MatchLineModel()
            {
                index= i,
                PlayerWeight = 0,
                OpponentWeight = 0,
                Status = MatchLineModel.LineState.inProgress,
                PlayerSelected = false,
                PlayerSpawnPoint = m_PlayerSpawnPoints[i],
                OpponentSpawnPoint = m_OpponentSpawnPoints[i]
            });
        }
    }

    /// <summary>
    /// Used to check if a player has enough money for a unit
    /// </summary>
    /// <param name="Model"></param>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public bool CheckForBalance(UnitModel Model, bool isPlayer = true)
    {
        if (isPlayer)
            return PlayerMoney - Model.Cost >= 0;
        else
            return OpponentMoney - Model.Cost >= 0;
    }

    /// <summary>
    /// Add money to balance
    /// </summary>
    /// <returns></returns>
    IEnumerator IncrementMoney()
    {
        for (; ; )
        {
            PlayerMoney += 10;
            OpponentMoney += 10;
            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Decrement player money
    /// </summary>
    /// <param name="Model"></param>
    public void DecrementMoney(UnitModel Model)
    {
        PlayerMoney -= Model.Cost;
    }

    /// <summary>
    /// Check if two lines or more are over
    /// if true the game end
    /// </summary>
    public void CheckForGameEnd()
    {
        if (MatchLines.FindAll(x=>x.Status != MatchLineModel.LineState.inProgress).Count>=2)
        {
            int playerLines = MatchLines.FindAll(x => x.Status == MatchLineModel.LineState.playerWon).Count;
            int opponentLines = MatchLines.FindAll(x => x.Status == MatchLineModel.LineState.OpponentWon).Count;

            this.GameStatus = playerLines > opponentLines ? MatchGameStatus.playerWon : MatchGameStatus.opponentWon;

            foreach(MatchLineModel m in MatchLines)
            {
                m.Status = MatchLineModel.LineState.playerWon;
            }
            DisplayMatchEnd();
        }
          
    }

    /// <summary>
    /// Used to destroy all uniut in order to clear the game
    /// </summary>
    public void DestroyAllUnits()
    {
        foreach (Transform child in UnitContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Register if the player or the opponent kill a unit for the stat
    /// </summary>
    /// <param name="isPlayer"></param>
    public void RegisterKill(bool isPlayer)
    {
        if(isPlayer)
            MatchStatsManager.Instance.IncrementUnitKilled();
    }

    /// <summary>
    /// Register if the player or the opponent win a line for the stat
    /// </summary>
    /// <param name="isPlayer"></param>
    public void RegisterLine(bool isPlayer)
    {
        if (isPlayer)
            MatchStatsManager.Instance.IncrementLineWon();
    }

    /// <summary>
    /// Display the match end interface
    /// </summary>
    public void DisplayMatchEnd()
    {
        m_matchHasEnded = true;
        m_matchEndView.gameObject.SetActive(true);
        int gain = MatchStatsManager.Instance.ComputeMatchGain();
        m_matchEndView.Populate(MatchStatsManager.Instance.MatchStats);
        CurrencyManager.Instance.AddPlayerCurrency(gain);
    }
}
