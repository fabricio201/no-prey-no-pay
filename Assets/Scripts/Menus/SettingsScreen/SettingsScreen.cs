﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the settings screen behaviour
/// </summary>
public class SettingsScreen : MonoBehaviour {

    [SerializeField] private MenuInputController m_input;
	[SerializeField] private MenuCity MenuAnimator;



	[Header("Screen references")]
	[SerializeField] private GameObject MainMenuScreen;
	
	void Awake(){
		m_input = transform.parent.GetComponent<MenuInputController>();
	}

	void Update () {
		if(m_input.GetPrevious()){
			MenuActions.instance.ChangePanel(MainMenuScreen);
			MenuAnimator.goBack();
		}
	}
}
