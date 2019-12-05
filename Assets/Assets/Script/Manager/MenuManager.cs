using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contain all the method in order to navigate between menus
/// </summary>
public class MenuManager : MonoBehaviour {
    
    public enum GlobaState{
        inMainMenu,
        inMatch,
        inProgressionScreen
    }

    [SerializeField]
    private GameObject m_Match;
    [SerializeField]
    private GameObject m_PlayButton;
    [SerializeField]
    private GameObject m_QuitButton;
    [SerializeField]
    private GameObject m_ProgressButton;
    [SerializeField]
    private GameObject m_EndObjectsContainer;
    [SerializeField]
    private GameObject m_Lines;
    [SerializeField]
    private MatchEndView m_matchEndView;
    [SerializeField]
    private UnitProgressionController m_progressionController;

    [SerializeField]
    private GameManager m_gameManager;

    [NonSerialized]
    public static GlobaState GlobalState;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame()
    {
        m_EndObjectsContainer.SetActive(true);
        m_Lines.SetActive(true);
        m_Match.SetActive(true);
        m_PlayButton.SetActive(false);
        m_ProgressButton.SetActive(false);
        m_QuitButton.SetActive(true);
        GlobalState = GlobaState.inMatch;
        m_gameManager.Init();
    }

    public void QuitGame()
    {
        m_EndObjectsContainer.SetActive(false);
        m_Lines.SetActive(false);
        m_Match.SetActive(false);
        m_PlayButton.SetActive(true);
        m_QuitButton.SetActive(false);
        m_matchEndView.gameObject.SetActive(false);
        m_ProgressButton.SetActive(true);
        m_progressionController.gameObject.SetActive(false);
        GlobalState = GlobaState.inMainMenu;
        m_gameManager.Quit();
    }

    public void GoToProgressionView()
    {
        m_progressionController.gameObject.SetActive(true);
        m_ProgressButton.SetActive(false);
        m_PlayButton.SetActive(false);
        m_QuitButton.SetActive(true);
        GlobalState = GlobaState.inProgressionScreen;
        m_progressionController.Initialize();
    }
}
