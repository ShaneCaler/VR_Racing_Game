using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKartControllerOLD : MonoBehaviour
{
	[Header("Vehicle Information")]
	public float maxSteerAngle = 40f;
	public float turnSpeed = 5f;
	public float maxSpeed;
	public Vector3 centerOfMass;

	[Header("Wheels and Lights")]
	public WheelCollider[] steeringWheels;
	public WheelCollider[] throttlingWheels;
	public GameObject[] wheelMeshes;


	private Rigidbody rb;
	private float currSpeed;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		//rb.centerOfMass = centerOfMass;
	}

	public void Steer(float targetSteerAngle)
	{
		//Vector3 relVector = transform.InverseTransformPoint(currentNode.position);
		//float wheelAngle = (relVector.x / relVector.magnitude) * maxSteerAngle; // magnitude = length

		//targetSteerAngle = wheelAngle;

		for (int i = 0; i < steeringWheels.Length; i++)
			steeringWheels[i].steerAngle = targetSteerAngle;

		for (int i = 0; i < wheelMeshes.Length; i++)
		{
			wheelMeshes[i].transform.Rotate(rb.velocity.magnitude
				* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * 5f
				/ (2 * Mathf.PI * .33f), 0f, 0f); // .33 = radius
		}
	}

	public void Drive(float motorStrength)
	{
		//currSpeed = 2f * Mathf.PI * throttlingWheels[1].radius * throttlingWheels[1].rpm * 60f / 1000f;
		for (int i = 0; i < throttlingWheels.Length; i++)
		{
			//if (currSpeed < maxSpeed)
				throttlingWheels[i].motorTorque = motorStrength;
			//else
				//throttlingWheels[i].motorTorque = 0f;
		}
	}

}
