namespace VRTK.Examples
{
	using UnityEngine;
	using System.Collections.Generic;
	public class KartV3 : MonoBehaviour
	{

		private Vector2 touchAxis;
		private float triggerAxis;
		private Rigidbody rb;
		private int count = 0;
		public List<WheelCollider> throttlingWheels;
		public List<GameObject> steeringWheels;
		public List<GameObject> wheelMeshes;
		public float strengthCoefficient = 20000f;
		public float maxTurn = 40f; // in degrees
		public float wheelSpinSpeed = 5f;
		public Transform centerOfMass;

		void Start()
		{
			//review center of mass docs
			//rb.centerOfMass = centerOfMass.position;
			Physics.IgnoreLayerCollision(9, 10);
			Physics.IgnoreLayerCollision(9, 11);

		}

		public void ResetCar()
		{
			Debug.Log("Resetting car");
		}

		public void SetTouchAxis(Vector2 data)
		{
			touchAxis = data;
		}

		public void SetTriggerAxis(float data)
		{
			triggerAxis = data;
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		void FixedUpdate()
		{
			Debug.Log("Trigger axis = " + triggerAxis.ToString() + " and touch axis = y: " + touchAxis.y + " / x: " + touchAxis.x);

			foreach(WheelCollider wheel in throttlingWheels)
			{
				Vector3 pos;
				Quaternion rot;
				wheel.motorTorque = strengthCoefficient * touchAxis.y * Time.deltaTime;
				//wheel.GetWorldPose(out pos, out rot);
				//wheelMeshes[count].transform.rotation = rot;
				//count++;
			}

			count = 0;

			foreach(GameObject wheel in steeringWheels)
			{
				wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * touchAxis.x;
				wheel.transform.localEulerAngles = new Vector3(0f, touchAxis.x * maxTurn, 0f);
			}

			foreach (GameObject wheelMesh in wheelMeshes)
			{
				wheelMesh.transform.Rotate(rb.velocity.magnitude 
					* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * wheelSpinSpeed
					/ (2 * Mathf.PI * .35f), 0f, 0f); // .33 = radius
			}
		}
	}
}