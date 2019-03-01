using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine : MonoBehaviour
{
	public Transform path;
	public float maxSteerAngle = 40f;
	public float motorStrength = 50f;
	public float maxSpeed;
	public Vector3 centerOfMass;
	public WheelCollider[] steeringWheels;
	public WheelCollider[] throttlingWheels;
	public GameObject[] wheelMeshes;

	private List<Transform> nodes;
	private int currNode = 0;
	private Rigidbody rb;
	private float currSpeed;

    private void Start()
    {
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = centerOfMass;
		Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();

		for (int i = 0; i < pathTransforms.Length; i++)
		{
			if (pathTransforms[i] != path.transform)
				nodes.Add(pathTransforms[i]);
		}
	}

    private void FixedUpdate()
    {
		Steer();
		Drive();
		Navigate();
    }

	private void Steer()
	{
		Vector3 relVector = transform.InverseTransformPoint(nodes[currNode].position);
		float wheelAngle = (relVector.x / relVector.magnitude) * maxSteerAngle; // magnitude = length

		for(int i = 0; i < steeringWheels.Length; i++)
			steeringWheels[i].steerAngle = wheelAngle;

		for (int i = 0; i < wheelMeshes.Length; i++)
		{
			wheelMeshes[i].transform.Rotate(rb.velocity.magnitude
				* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * 5f
				/ (2 * Mathf.PI * .33f), 0f, 0f); // .33 = radius
		}
	}

	private void Drive()
	{
		currSpeed = 2f * Mathf.PI * throttlingWheels[1].radius * throttlingWheels[1].rpm * 60f / 1000f;
		for(int i = 0; i < throttlingWheels.Length; i++)
		{
			if (currSpeed < maxSpeed)
				throttlingWheels[i].motorTorque = motorStrength;
			else
				throttlingWheels[i].motorTorque = 0f;
		}
	}

	private void Navigate()
	{
		if (Vector3.Distance(transform.position, nodes[currNode].position) < 0.8f)
		{
			if (currNode == nodes.Count - 1)
				currNode = 0;
			else
				currNode++;
		}
	}
}
