using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class EnemyAgentOLD : Agent
{
	public float action_steer = 0f; 
	public float action_throttle = 0f;
	public Transform[] waypoints;

	private float currentNode = 0f;
	private bool collided = false;
	private bool offTrack = false;
	private bool startLineFlag = false;
	private EnemyKartController kart;

	void Start()
    {
		kart = GetComponent<EnemyKartController>();
    }

    void Update()
	{
		Debug.Log("square mag: " + GetComponent<Rigidbody>().velocity.sqrMagnitude);
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
		transform.SetPositionAndRotation(waypoints[index - 1].position, new Quaternion(0, 0, 0, 0));
		transform.LookAt(waypoints[index].position);

		//reset kart speed to zero
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		collided = false;
		offTrack = false;
		action_steer = 0;
		action_throttle = 0;
		//Debug.Log("Reset complete");
	}

	public override void CollectObservations()
	{
		
	}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		action_steer = Mathf.Clamp(vectorAction[0], -1f, 1f);
		action_throttle = 1;
		//Debug.Log("action_steer: " + action_steer + " action_throttle: " + action_throttle);

		if (collided || offTrack || transform.position.y < .3f)
		{
			Debug.Log("Done!");
			AddReward(-1.0f);
			Done();
		}
		else if(GetComponent<Rigidbody>().velocity.sqrMagnitude > 25f)
		{
			AddReward(0.1f);
		}
		else
		{
			AddReward(0.05f);
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
		else //we hit a wall -> collision detected -> end of episode
		{
			Debug.Log("collided!");
			collided = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		startLineFlag = false;
	}
}
