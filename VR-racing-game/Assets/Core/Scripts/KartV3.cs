﻿namespace VRTK
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using VRTK.Controllables;

	public class KartV3 : MonoBehaviour
	{

		private Vector2 touchAxis;
		private float triggerAxis;
		private int controllerRef;
		private Rigidbody rb;
		private float steerValue;
		private float brake = 0f;
		private Lighting_Manager lm;
		private Camera_Controller cam;

		public VRTK_BaseControllable steeringWheel;
		public List<WheelCollider> throttlingWheels;
		public List<GameObject> steeringWheels;
		public List<GameObject> wheelMeshes;
		public float torqueStrength = 20000f;
		public float brakeStrength = 10f;
		public float maxTurn = 40f; // in degrees
		public float wheelSpinSpeed = 5f;
		public float healthPoints = 100f;
		public int maxPowerups = 2;
		public Transform centerOfMass;
		
		void Start()
		{
			//review center of mass docs
			//rb.centerOfMass = centerOfMass.position;
			Physics.IgnoreLayerCollision(9, 10);
			Physics.IgnoreLayerCollision(9, 11);
			OVRManager.display.RecenterPose();
			lm = GetComponent<Lighting_Manager>();
			cam = GetComponentInChildren<Camera_Controller>();
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

		public void SetTriggerAxis(float data, VRTK_ControllerReference controller)
		{
			triggerAxis = data;
			Debug.Log("Controller: " + controller.index);
			controllerRef = (int)controller.index;
		}
		
		// Power Up functions
		public void ApplySpeedBoost(float str, float dur)
		{
			StartCoroutine(HandleSpeedBoost(str, dur));
		}

		IEnumerator HandleSpeedBoost(float str, float dur)
		{
			torqueStrength += str;
			yield return new WaitForSeconds(dur);
			torqueStrength -= str;
		}

		public void SetHealthPoints(float data)
		{
			healthPoints += data;
		}

		public void ApplyOvershield(GameObject overshield, float duration)
		{
			Debug.Log("Applying Overshield...");
			GameObject os = Instantiate(overshield, transform.position, Quaternion.identity);
			os.transform.parent = transform;
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}

		void Update()
		{
			/* if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0f && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 0f)
			{
				lm.ToggleBrakeLights();
				brake = triggerAxis;
			}
			else
			{
				lm.ToggleBrakeLights();
				brake = 0f;
			} */
		}

		void FixedUpdate()
		{
			//Debug.Log("Trigger axis = " + triggerAxis.ToString() + " and touch axis = y: " + touchAxis.y + " / x: " + touchAxis.x);

			// move forward and brake
			foreach(WheelCollider wheel in throttlingWheels)
			{
				if (controllerRef == 0)
				{
					Debug.Log("Applying brake torque");
					wheel.motorTorque = 0f;
					wheel.brakeTorque = brakeStrength * Time.deltaTime;
				}
				else
				{
					Debug.Log("Applying motor torque");
					wheel.brakeTorque = 0f;
					wheel.motorTorque = torqueStrength * triggerAxis * Time.deltaTime;
				}
			}

			// steer
			foreach(GameObject wheel in steeringWheels)
			{
				wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * touchAxis.x;
				//wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * steerValue * Time.deltaTime;
				//Debug.Log("Steer angle: " + wheel.GetComponent<WheelCollider>().steerAngle.ToString());
				//wheel.transform.localEulerAngles = new Vector3(0f, steerValue * maxTurn * Time.deltaTime, 0f);
				wheel.transform.localEulerAngles = new Vector3(0f, touchAxis.x * maxTurn, 0f);

			}

			// rotate wheels
			foreach (GameObject wheelMesh in wheelMeshes)
			{
				wheelMesh.transform.Rotate(rb.velocity.magnitude 
					* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * wheelSpinSpeed
					/ (2 * Mathf.PI * .33f), 0f, 0f); // .33 = radius
			}
		}
	}
}