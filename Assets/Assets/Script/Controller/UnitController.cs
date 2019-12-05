using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    [SerializeField]
    private UnitModel m_model;

    [SerializeField]
    private Animator m_animator;

    [NonSerialized]
    public Transform SpawnPoint;
    [NonSerialized]
    public Transform TargetPoint;

    [NonSerialized]
    public GameManager GameManager;

    [SerializeField]
    private UnitMaterialSwitcher m_materialSwitcher;

    public bool IsPlayer;
    public int LineIndex;

    public bool isAttacking;
    public float life;

    public bool IsEndObject;

    private GameObject m_closestEnnemy = null;

    private UnitController[] m_ennemies;

    private void Start()
    {
        SetUnitMaterial();
        if(m_animator)
            m_animator.speed = m_model.GetRealLevel(IsPlayer);

        if (GameManager == null && MenuManager.GlobalState == MenuManager.GlobaState.inMatch)
            GameManager = FindObjectsOfType<GameManager>()[0];

        if (m_model)
            life = m_model.LifePoints * m_model.GetRealLevel(IsPlayer);
    }

    // Update is called once per frame
    void Update () {
        if (MenuManager.GlobalState != MenuManager.GlobaState.inMatch)
            return;

        MoveToTarget();
        CheckForAttack();
        CheckForDeath();
        CheckForFinalAnimation();
    }

    public void MoveToTarget()
    {
        // First check for line status
        if (IsEndObject || isAttacking || GameManager.MatchLines[LineIndex].Status != MatchLineModel.LineState.inProgress)
            return;

        // Set variables for the animator
        float step = 0;
        if (m_model)
            step = m_model.Speed * Time.deltaTime * m_model.GetRealLevel(IsPlayer);
        if (m_animator)
        {
            m_animator.SetFloat("speed", 1);
        }

        if (TargetPoint == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPoint.transform.position, step); // move to target default point
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPoint.position, step); // move to target point
            transform.position = new Vector3(transform.position.x, SpawnPoint.position.y, transform.position.z); // keep y position
        }
    }

    public void CheckForAttack()
    {
        isAttacking = false;
        if(m_animator)
            m_animator.SetBool("attack", false);
        // Check for line status 
        if (!IsEndObject && GameManager.MatchLines[LineIndex].Status != MatchLineModel.LineState.inProgress || IsEndObject)
            return;

        // First get all ennemies in the line
        if (IsPlayer)
            m_ennemies = Array.FindAll(FindObjectsOfType<UnitController>(), x => x.IsPlayer == false && x.LineIndex == LineIndex); // get all ennemies on the same line
        else
            m_ennemies = Array.FindAll(FindObjectsOfType<UnitController>(), x => x.IsPlayer == true && x.LineIndex == LineIndex);

        float distance = Mathf.Infinity;

        // Then check for the nearest
        foreach (UnitController u in m_ennemies)
        {
            Vector3 diff = (u.gameObject.transform.position - transform.position);
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                m_closestEnnemy = u.gameObject;
                distance = curDistance;
            }
        }

        // If the ennemy is near enough, let's attack
        if (distance < 0.003 && m_animator != null && m_closestEnnemy)
        {
            m_animator.SetBool("attack", true);
            m_animator.SetFloat("speed", 0);
            isAttacking = true;
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("hasAttack"))
            {
                m_closestEnnemy.GetComponent<UnitController>().life -= m_model.AttackPoints * m_model.GetRealLevel(IsPlayer);
            }

            // if ennemy is destroy continue
            if (m_closestEnnemy.GetComponent<UnitController>().life <= 0)
            {
                m_closestEnnemy = null;
                distance = Mathf.Infinity;
                isAttacking = false;
                m_animator.SetFloat("speed", 1);
                m_animator.SetBool("attack", false);
            }
        }
    }

    public void CheckForDeath()
    {
        if (life <= 0)
        {
            // Check if is end object
            if (IsEndObject)
            {
                SetLineStatusWhenOver();
            }
            GiveMoney();
            // if unit is opponent
            GameManager.RegisterKill(!IsPlayer);

            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// When a final object is dead set the line status
    /// </summary>
    public void SetLineStatusWhenOver()
    {
        if (GameManager == null)
            GameManager = FindObjectsOfType<GameManager>()[0];

        GameManager.MatchLines[LineIndex].Status = IsPlayer ? MatchLineModel.LineState.OpponentWon : MatchLineModel.LineState.playerWon;

        // if unit is opponent
        GameManager.RegisterLine(!IsPlayer);
    }

    /// <summary>
    /// Used to give money to the player when a unit die
    /// </summary>
    public void GiveMoney()
    {
        if (IsPlayer)
            GameManager.OpponentMoney += m_model!=null ? m_model.MoneyEarn : GameManager.LineFinalMoney;
        else
            GameManager.PlayerMoney += m_model != null ? m_model.MoneyEarn : GameManager.LineFinalMoney;
    }

    /// <summary>
    /// Used to set animation when a line is over
    /// </summary>
    public void CheckForFinalAnimation()
    {
        if (GameManager == null)
            return;

        if ((GameManager.MatchLines[LineIndex].Status != MatchLineModel.LineState.inProgress) || GameManager.GameStatus != GameManager.MatchGameStatus.inProgress)
            SetFinalAnimation();
    }


    /// <summary>
    /// Used to set animation when the game is over
    /// </summary>
    public void SetFinalAnimation()
    {
        if (m_animator == null)
            return;

        m_animator.SetFloat("speed", 0);
        m_animator.SetBool("isOver", true);

        if (GameManager.MatchLines[LineIndex].Status == MatchLineModel.LineState.playerWon && IsPlayer)
            m_animator.SetBool("won", true);
        if(GameManager.MatchLines[LineIndex].Status == MatchLineModel.LineState.OpponentWon && IsPlayer)
            m_animator.SetBool("won", false);

        if (GameManager.MatchLines[LineIndex].Status == MatchLineModel.LineState.OpponentWon && !IsPlayer)
            m_animator.SetBool("won", true);
        if (GameManager.MatchLines[LineIndex].Status == MatchLineModel.LineState.playerWon && !IsPlayer)
            m_animator.SetBool("won", false);
    }

    /// <summary>
    /// Change material if player or opponent
    /// </summary>
    public void SetUnitMaterial()
    {
        if(m_materialSwitcher)
            m_materialSwitcher.SetMaterial(IsPlayer);
    }
}
