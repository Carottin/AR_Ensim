﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CurrencyManager.Instance.Intialize(); // Initialize the currency manager
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
