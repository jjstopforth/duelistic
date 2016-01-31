﻿using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

	public AttractorController Player1Attractor;
    public float Player1DecisionConfidence;
	public AttractorController Player2Attractor;
	public float Player2DecisionConfidence;

	public AttractorTarget GunTarget1;
	public AttractorTarget GunTarget2;

	// Use this for initialization
	void Start () 
	{
        Reset();
	}

    public void Reset()
    {
        Player1DecisionConfidence = 0f;
        Player2DecisionConfidence = 0f;
    }
	
	// Update is called once per frame
	void Update ()
	{

		//Update Race!
		float dt = Time.deltaTime;

		if (Player1Attractor.HasTarget) {
			Player1DecisionConfidence += dt / 2f;
		}


		if (Player2Attractor != null) { //Remove later
			if (Player2Attractor.HasTarget) {
				Player2DecisionConfidence += dt / 2f;
			}
		}
	
	}

	public void ResetPlayerHand (string playerName)
	{	

		if (playerName == "player1") {
			Player1Attractor.ResetTarget();
			Player1DecisionConfidence = 0f;
		} 
		else if (playerName == "player2") 
		{
			Player2Attractor.ResetTarget();
			Player2DecisionConfidence = 0f;
		} 
		else 
		{
			Debug.Log("Invalid player name.");
		}

	}

	public void SetHandTarget (string playerName)
	{

		AttractorTarget target;

		if (playerName == "player1") {
			if (!Player1Attractor.HasTarget) {
				target = FindNearestTarget(Player1Attractor, Player2Attractor);
				Player1Attractor.SetTarget (target.transform.localPosition);
				target.Attractor = Player1Attractor;
			}
			
		} 
		else if (playerName == "player2") 
		{
			if (!Player2Attractor.HasTarget) {
				target = FindNearestTarget(Player2Attractor, Player1Attractor);
				Player2Attractor.SetTarget (target.transform.localPosition);
				target.Attractor = Player2Attractor;
			}
		}
		
	}

	AttractorTarget FindNearestTarget (AttractorController source, AttractorController competitor)
	{	
		float distToTarget1 = Vector3.Distance (source.transform.localPosition, GunTarget1.transform.localPosition);
		float distToTarget2 = Vector3.Distance (source.transform.localPosition, GunTarget2.transform.localPosition);

		if (distToTarget1 <= distToTarget2) {
			if (competitor.HasTarget && Vector3.Equals (competitor.TargetPosition, GunTarget1.transform.localPosition)) 
			{
				return GunTarget2;
			}
			return GunTarget1;
		} 
		else 
		{
			if (competitor.HasTarget && Vector3.Equals (competitor.TargetPosition, GunTarget2.transform.localPosition)) 
			{
				return GunTarget1;
			}
			return GunTarget2;
		}
	}  
}
