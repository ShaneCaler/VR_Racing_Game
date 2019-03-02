using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine : MonoBehaviour
{
	[Header("Vehicle Information")]
	public float maxSteerAngle = 40f;
	public float turnSpeed = 5f;
	public float motorStrength = 50f;
	public float brakeStrength = 150f;
	public float maxSpeed;
	public Vector3 centerOfMass;
	public Transform path;

	[Header("Wheels and Lights")]
	public WheelCollider[] steeringWheels;
	public WheelCollider[] throttlingWheels;
	public WheelCollider[] brakingWheels;
	public GameObject[] wheelMeshes;
	public Light[] headLights;
	public Light[] brakeLights;

	[Header("Sensors")]
	public float sensorLength = 3f;
	public float frSensorAngle = 30f;
	public float frSideSensorOffset = .5f;
	public float frSensorAvoidMultiplier = 1f;
	public float frAngledSensorAvoidMultiplier = .5f;
	public Vector3 frSensorPosition = new Vector3(.3f, 0f, .5f);

	
	private List<Transform> nodes;
	private int currNode = 0;
	private Rigidbody rb;
	private float currSpeed;
	private bool isBraking = false;
	private bool isAvoiding = false;
	private float targetSteerAngle = 0f;

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
		Sensors();
		Steer();
		Drive();
		Navigate();
		Braking();
		LerpToSteerAngle();
    }

	private void Sensors()
	{
		RaycastHit hit;
		Vector3 sensorStartingPos = transform.position;
		sensorStartingPos += transform.forward * frSensorPosition.z;
		sensorStartingPos += transform.up * frSensorPosition.y;
		float avoidMultiplier = 0f;
		isAvoiding = false;

		// front right sensor
		sensorStartingPos += transform.right * frSideSensorOffset;
		if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
		{
			if (!hit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(sensorStartingPos, hit.point, Color.blue);
				isAvoiding = true;
				avoidMultiplier -= frSensorAvoidMultiplier;
			}
		}

		// front right angled sensor
		else if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(frSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
		{
			if (!hit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(sensorStartingPos, hit.point, Color.yellow);
				isAvoiding = true;
				avoidMultiplier -= frAngledSensorAvoidMultiplier;
			}
		}

		// front left sensor
		sensorStartingPos -= 2 * transform.right * frSideSensorOffset;
		if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
		{
			if (!hit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(sensorStartingPos, hit.point, Color.blue);
				isAvoiding = true;
				avoidMultiplier += frSensorAvoidMultiplier;
			}
		}

		// front left angled sensor
		else if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(-frSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
		{
			if (!hit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(sensorStartingPos, hit.point, Color.yellow);
				isAvoiding = true;
				avoidMultiplier += frAngledSensorAvoidMultiplier;

			}
		}

		// front center sensor
		if(avoidMultiplier == 0)
		{
			if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
			{
				if (!hit.collider.CompareTag("Terrain"))
				{
					Debug.DrawLine(sensorStartingPos, hit.point, Color.red);
					isAvoiding = true;
					if (hit.normal.x < 0)
						avoidMultiplier = -frSensorAvoidMultiplier;
					else
						avoidMultiplier = frSensorAvoidMultiplier;

				}
			}
		}


		// check if we are avoiding something
		if (isAvoiding)
		{
			targetSteerAngle = maxSteerAngle * avoidMultiplier;
		}

	}

	private void Steer()
	{
		if (isAvoiding)
			return;

		Vector3 relVector = transform.InverseTransformPoint(nodes[currNode].position);
		float wheelAngle = (relVector.x / relVector.magnitude) * maxSteerAngle; // magnitude = length

		targetSteerAngle = wheelAngle;

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
			if (currSpeed < maxSpeed && !isBraking)
				throttlingWheels[i].motorTorque = motorStrength;
			else
				throttlingWheels[i].motorTorque = 0f;
		}
	}

	private void Navigate()
	{
		if (Vector3.Distance(transform.position, nodes[currNode].position) < 2f)
		{
			if (currNode == nodes.Count - 1)
				currNode = 0;
			else
				currNode++;
		}
	}

	private void Braking()
	{
		if (isBraking)
		{
			ToggleBrakeLights(2);
			for (int i = 0; i < brakingWheels.Length; i++)
			{
				brakingWheels[i].brakeTorque = brakeStrength;
			}
		}
		else
		{
			ToggleBrakeLights(0);
			for (int i = 0; i < brakingWheels.Length; i++)
			{
				brakingWheels[i].brakeTorque = 0f;
			}
		}
	}

	// enable when entering dark spaces
	private void ToggleHeadLights(int intensity)
	{
		for (int i = 0; i < headLights.Length; i++)
		{
			headLights[i].intensity = intensity;
		}
	}

	// enable only when braking
	private void ToggleBrakeLights(int intensity)
	{
		for(int i = 0; i < brakeLights.Length; i++)
		{
			brakeLights[i].intensity = intensity;
		}
	}

	private void LerpToSteerAngle()
	{
		for (int i = 0; i < steeringWheels.Length; i++)
			steeringWheels[i].steerAngle = Mathf.Lerp(steeringWheels[i].steerAngle, targetSteerAngle, turnSpeed * Time.deltaTime);
	}
}
