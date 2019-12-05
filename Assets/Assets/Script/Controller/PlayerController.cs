using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public static bool CoolDownIsOver = true;
    public static bool PowerCoolDownIsOver = false;

    public static bool IsInPower = false;

    private const string m_linetag1 = "Line1";
    private const string m_linetag2 = "Line2";
    private const string m_linetag3 = "Line3";

    [SerializeField]
    private Image m_powerScope;

    [SerializeField]
    private GameManager m_gameManager;
    [SerializeField]
    private List<ActiveLineSwitcher> m_lines;
    [SerializeField]
    private GameObject m_explosion;


    [Header("Inventory")]
    [SerializeField]
    private CardView m_card;
    [SerializeField]
    private GameObject m_cardsContainer;
    [SerializeField]
    private Text m_PlayerMoney;
    [SerializeField]
    SliderView m_coolDownSlider;
    [SerializeField]
    SliderView m_powerSlider;

    /// <summary>
    /// Used to reset all player's variables
    /// </summary>
    public void Init()
    {
        ClearInventory();
        IsInPower = false;
        CoolDownIsOver = true;
        PowerCoolDownIsOver = false;
        StartCoroutine(WaitUntilPowerCooldown(20));
        SetActiveLine(0);
        SetInventory();
    }

    /// <summary>
    /// Each frame, get the player active line
    /// Check if the player activate his power
    /// Update his money
    /// </summary>
    private void Update()
    {
        GetPlayerActiveLine();
        Shoot();
        UpdateBalance();
    }

    /// <summary>
    ///  Used to spawn a unit
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="Model"></param>
    public void SpawnUnit(GameObject Unit, UnitModel Model)
    {
        // Check if the player can spawn a unit
        if (!m_gameManager.CheckForBalance(Model) || !CoolDownIsOver || m_gameManager.GameStatus != GameManager.MatchGameStatus.inProgress)
            return;

        /// Decrement the money
        m_gameManager.DecrementMoney(Model);
        // Get the active line
        MatchLineModel activeLine = m_gameManager.MatchLines.Find(x => x.PlayerSelected == true);

        if (activeLine.Status != MatchLineModel.LineState.inProgress)
            return;

        // spawn a unit and set all its variables
        var unit = Instantiate(Unit, activeLine.PlayerSpawnPoint.transform.position, activeLine.PlayerSpawnPoint.transform.rotation, m_gameManager.UnitContainer.transform);
        unit.GetComponent<UnitController>().SpawnPoint = activeLine.PlayerSpawnPoint.transform;
        unit.GetComponent<UnitController>().TargetPoint = activeLine.OpponentSpawnPoint.transform;
        unit.GetComponent<UnitController>().IsPlayer = true;
        unit.GetComponent<UnitController>().LineIndex = activeLine.index;
        unit.GetComponent<UnitController>().GameManager =  this.m_gameManager;

        // Increment thhe line's weight
        activeLine.PlayerWeight += Model.Weight;

        // Start cooldown before the player can spawn another unit
        StartCoroutine(WaitUntilCooldown(3));
    }

    public IEnumerator WaitUntilCooldown(int timeToWait)
    {
        // reset the cooldown slider
        m_coolDownSlider.Reset();
        CoolDownIsOver = false;
        // wait before the player can spawn another unit
        yield return new WaitForSeconds(timeToWait);
        CoolDownIsOver = true;
    }

    /// <summary>
    /// Same as waitunitlcooldown but for the power
    /// </summary>
    /// <param name="timeToWait"></param>
    /// <returns></returns>
    public IEnumerator WaitUntilPowerCooldown(int timeToWait)
    {
        m_powerSlider.Reset();
        PowerCoolDownIsOver = false;
        yield return new WaitForSeconds(timeToWait);
        PowerCoolDownIsOver = true;
    }

    /// <summary>
    /// Used to wait before any action
    /// </summary>
    /// <param name="timeToWait"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator Tempo(float timeToWait, Action callback)
    {
        Debug.Log("start tempo");
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("Tempo is over");
        callback.Invoke();
    }

    /// <summary>
    /// Used to wait a certain amount of time between the power is activated and the player can shoot
    /// </summary>
    public void SetPowerUI()
    {
        if(PowerCoolDownIsOver && !IsInPower)
        {
            m_powerScope.gameObject.SetActive(true);
            StartCoroutine(Tempo(.5f, () =>
            {
                Debug.Log("CallbackInvoked");
                IsInPower = true;
            }));
        }
    }

    public void Shoot()
    {
        if (!IsInPower)
            return;

        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform);
                GameObject projectile = Instantiate(m_explosion, hit.point, hit.transform.rotation) as GameObject;
                m_powerScope.gameObject.SetActive(false);
                IsInPower = false;
                StartCoroutine(WaitUntilPowerCooldown(20));
            }
        }
    }


    public void GetPlayerActiveLine()
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
#if !UNITY_EDITOR
            if (Input.touches.Length <= 0 || !ARController.IsPosed)
                return;


            Touch touch = Input.touches[0];

            // if touch is on gui return last active spawn point
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;


            Ray ray = Camera.main.ScreenPointToRay(touch.position);
#endif

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButton(0))
        {
            string tag = hit.collider.tag;
            Debug.Log("tag is : " + tag);
            switch (tag)
            {
                case m_linetag1:
                    SetActiveLine(0);
                    break;
                case m_linetag2:
                    SetActiveLine(1);
                    break;
                case m_linetag3:
                    SetActiveLine(2);
                    break;
                default:
                    SetActiveLine(0);
                    break;
            }
        }

#if UNITY_EDITOR
        /*if (ActiveSpawnPoint == null || ActiveTargetPoint == null)
        {
            ActiveSpawnPoint = PlayerSpawnPoints[0];
            ActiveTargetPoint = OpponentSpawnPoints[0];
        }*/
#endif
    }

    public void SetActiveLine(int index)
    {
        for (int i = 0; i < m_lines.Count; i++)
        {
            if (i == index)
            {
                m_lines[i].SetActive();
                m_gameManager.MatchLines[i].PlayerSelected = true;
            }
            else
            {
                m_lines[i].SetInactive();
                m_gameManager.MatchLines[i].PlayerSelected = false;
            }
        }

    }

    public void SetInventory()
    {
        // Add Cards to inventory
        foreach (UnitModel um in m_gameManager.Inventory)
        {
            var card = Instantiate(m_card, m_cardsContainer.transform);
            card.Player = this;
            card.Populate(um);
            card.Unit = um.gameObject;
        }
    }

    public void UpdateBalance()
    {
        m_PlayerMoney.text = m_gameManager.PlayerMoney.ToString();
    }

    public void ClearInventory()
    {
        foreach (Transform child in m_cardsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

