﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStarter : MonoBehaviour {

	[SerializeField] private PlayerSpawner PSpawner;
	[SerializeField] private Countdown countdown;
	[SerializeField] private RoundReferee referee;

	// public WeaponInfo[] WeaponsToSpawn;
	public List<PlayerInfo> PlayersToSpawn;

	public int StockLimit;
	public float TimeLimit;

	public void InitializeRound(){
		PSpawner.PlayersToSpawn = PlayersToSpawn;
		PSpawner.SpawnPlayers();

		//spawn weapons

		countdown.StartCountdown();
	}
}
