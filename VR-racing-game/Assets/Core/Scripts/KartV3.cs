namespace VRTK
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using VRTK.Controllables;

	public class KartV3 : MonoBehaviour
	{

		private Vector2 touchAxis;
		private float triggerAxis;
		private Rigidbody rb;
		private int count = 0;
		private float steerValue;
		private float brake = 0f;

		public VRTK_BaseControllable steeringWheel;
		public List<WheelCollider> throttlingWheels;
		public List<GameObject> steeringWheels;
		public List<GameObject> wheelMeshes;
		public float torqueStrength = 20000f;
		public float brakeStrength = 10f;
		public float maxTurn = 40f; // in degrees
		public float wheelSpinSpeed = 5f;
		public int healthPoints = 100;
		public Transform centerOfMass;
		
		void Start()
		{
			//review center of mass docs
			//rb.centerOfMass = centerOfMass.position;
			Physics.IgnoreLayerCollision(9, 10);
			Physics.IgnoreLayerCollision(9, 11);

		}

		protected virtual void OnEnable()
		{
			steeringWheel = (steeringWheel == null ? GetComponent<VRTK_BaseControllable>() : steeringWheel);
			steeringWheel.ValueChanged += ValueChanged;
		}

		protected virtual void ValueChanged(object sender, ControllableEventArgs e)
		{
			//Debug.Log("value of e: " + e.value.ToString("F1"));
			steerValue = e.value;
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

		// Power Up functions
		public void SetTorqueStrength(float data)
		{
			torqueStrength += data;
		}

		public void SetHealthPoints(int data)
		{
			healthPoints += data;
		}

		public void ApplyOvershield(GameObject overshield, float duration)
		{
			Debug.Log("Applying Overshield...");
			Instantiate(overshield, transform.position, Quaternion.identity);
			overshield.transform.parent = transform;
			StartCoroutine(WaitToDestroyShield(overshield, duration));
		}

		IEnumerator WaitToDestroyShield(GameObject overshield, float duration)
		{
			yield return new WaitForSeconds(duration);
			Destroy(overshield);
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		void Update()
		{
			if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 0f)
			{
				brake = triggerAxis;
			}
			else
			{
				brake = 0f;
			}
		}

		void FixedUpdate()
		{
			Debug.Log("Trigger axis = " + triggerAxis.ToString() + " and touch axis = y: " + touchAxis.y + " / x: " + touchAxis.x);

			// move forward and brake
			foreach(WheelCollider wheel in throttlingWheels)
			{
				// apply torque
				wheel.motorTorque = torqueStrength * triggerAxis * Time.deltaTime;

				// brake
				wheel.brakeTorque = brakeStrength * brake * Time.deltaTime;
			}

			
			count = 0;

			// steer
			foreach(GameObject wheel in steeringWheels)
			{
				wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * touchAxis.x;
				//wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * steerValue * Time.deltaTime;
				Debug.Log("Steer angle: " + wheel.GetComponent<WheelCollider>().steerAngle.ToString());
				//wheel.transform.localEulerAngles = new Vector3(0f, steerValue * maxTurn * Time.deltaTime, 0f);
				wheel.transform.localEulerAngles = new Vector3(0f, touchAxis.x * maxTurn * Time.deltaTime, 0f);

			}

			// rotate wheels
			foreach (GameObject wheelMesh in wheelMeshes)
			{
				wheelMesh.transform.Rotate(rb.velocity.magnitude 
					* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * wheelSpinSpeed
					/ (2 * Mathf.PI * .35f), 0f, 0f); // .33 = radius
			}
		}
	}
}