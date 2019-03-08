using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class EnemyAgent : Agent
{
	public float action_steer = 0f;
	public float action_throttle = 0f;
	public float currentReward = 0f;
	public float currentSpeed;
	public Transform[] waypoints;
	public GameObject goal;

	private int currNode = 0;
	private float distance = 0f;
	private float prevThrottle = 0f;
	private float rewardMultiplier = 0f;
	private bool collided = false;
	private bool offTrack = false;
	private bool startLineFlag = false;
	private bool goalFlag = false;
	private EnemyKartController kart;
	private RayPerception rayPer;

	void Start()
	{
		kart = GetComponent<EnemyKartController>();
		rayPer = GetComponent<RayPerception>();
	}

	void Update()
	{
		currentSpeed = kart.CurrentSpeed;
	}

	void FixedUpdate()
	{
		CheckIfGrounded();
		kart.Move(action_steer, action_throttle, 0f, 0f);
	}

	private void CheckIfGrounded()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, 3f))
		{
			if (!hit.collider.CompareTag("ground"))
			{
				Debug.Log("No longer on track");
				Debug.DrawLine(transform.position, hit.point, Color.blue);
				offTrack = true;
			}
		}
	}

	public override void AgentReset()
	{
		float tmp_mindist = 1e+6f;
		int index = 0;
		for (int i = 1; i < waypoints.Length; i++)
		{
			float tmp_dist = Vector3.SqrMagnitude(waypoints[i].position - transform.position);
			if (tmp_dist < tmp_mindist)
			{
				tmp_mindist = tmp_dist;
				index = i;
			}
		}
		//restart from previous waypoint so that we try again failed section
		//TODO - can set index manually here, so to focus on specific sector of track
		transform.SetPositionAndRotation(waypoints[0].position + new Vector3(5f, 0f, 0f), new Quaternion(0, 0, 0, 0));
		transform.LookAt(waypoints[0].position);

		//reset kart speed to zero
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		collided = false;
		offTrack = false;
		goalFlag = false;
		action_steer = 0f;
		action_throttle = 0f;
		rewardMultiplier = 0f;
		currNode = 0;
	}

	public override void CollectObservations()
	{
		// use raycast instead of visual observations?
		//float rayDistance = 50f;
		//float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
		//string[] detectableObjects = { "agent", "ground", "Terrain", "Obstacle" };
		//AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

		//distance = Vector3.Distance(goal.transform.position, gameObject.transform.position);
		//AddVectorObs(distance);
		AddVectorObs(transform.position);
		AddVectorObs(goal.transform.position);
	}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		action_steer = Mathf.Clamp(vectorAction[0], -1f, 1f);
		action_throttle = Mathf.Clamp(vectorAction[1], 0f, 1f);

		if (collided || offTrack || transform.position.y < .3f)
		{
			currentReward = -1f;
			AddReward(currentReward);
			Done();
		}
		else if (goalFlag)
		{
			currentReward = 1;
			AddReward(currentReward);
			goalFlag = false;
		}
		else if(action_throttle == 0f)
		{
			//StartCoroutine(CheckIfStationary());
		}
		else if(action_throttle > 0f)
		{
			currentReward = action_throttle;
			AddReward(currentReward);
		}
	}

	IEnumerator CheckIfStationary()
	{
		prevThrottle = action_throttle;
		yield return new WaitForSeconds(5f);
		if(prevThrottle == 0 && action_throttle == 0)
		{
			currentReward = -.5f;
			Debug.Log("We're stationary :(");
			AddReward(currentReward);
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		//reset lap time when crossing start line
		if (other.CompareTag("StartLine"))
		{
			if (!startLineFlag)
			{
				startLineFlag = true;
			}
		}
		else if (other.CompareTag("goal"))
		{
			if (!goalFlag)
			{
				Debug.Log("Goal!!!");
				goalFlag = true;
			}
		}
		else //we hit a wall -> collision detected -> end of episode
		{
			Debug.Log("collided with " + other.gameObject);
			collided = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		startLineFlag = false;
	}
}
