using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLineModel : MonoBehaviour {

    public enum LineState
    {
        inProgress,
        playerWon,
        OpponentWon
    }

    public int index;

    public Transform PlayerSpawnPoint;
    public Transform OpponentSpawnPoint;

    public int PlayerWeight = 0;
    public int OpponentWeight = 0;

    public LineState Status;

    public bool PlayerSelected;
}
