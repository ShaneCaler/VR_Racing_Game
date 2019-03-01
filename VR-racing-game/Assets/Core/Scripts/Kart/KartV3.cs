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
		private int controllerRef;
		private Rigidbody rb;
		private float steerValue;
		private float brake = 0f;
		private Lighting_Manager lm;
		private Camera_Controller cam;
		private UI_Manager uim;

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
			uim = GetComponentInChildren<UI_Manager>();
		}

		protected virtual void OnEnable()
		{
			steeringWheel = (steeringWheel == null ? GetComponent<VRTK_BaseControllable>() : steeringWheel);
			steeringWheel.ValueChanged += ValueChanged;
		}

		protected virtual void ValueChanged(object sender, ControllableEventArgs e)
		{
			// Used for steering wheel controls
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
			uim.UpdateSpeedText(transform.InverseTransformVector(rb.velocity).z);
		}

		void FixedUpdate()
		{
			//Debug.Log("Trigger axis = " + triggerAxis.ToString() + " and touch axis = y: " + touchAxis.y + " / x: " + touchAxis.x);

			// move forward and brake
			for(int i = 0; i < throttlingWheels.Count; i++)
			{
				if (controllerRef == 0)
				{
					Debug.Log("Applying brake torque");
					throttlingWheels[i].motorTorque = 0f;
					throttlingWheels[i].brakeTorque = brakeStrength * Time.deltaTime;
					//lm.ToggleBrakeLights();
				}
				else
				{
					Debug.Log("Applying motor torque");
					throttlingWheels[i].brakeTorque = 0f;
					throttlingWheels[i].motorTorque = torqueStrength * triggerAxis * Time.deltaTime;
				}
			}

			// steer
			for(int i = 0; i < steeringWheels.Count; i++)
			{
				steeringWheels[i].GetComponent<WheelCollider>().steerAngle = maxTurn * touchAxis.x;
				//wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * steerValue * Time.deltaTime;
				//Debug.Log("Steer angle: " + wheel.GetComponent<WheelCollider>().steerAngle.ToString());
				//wheel.transform.localEulerAngles = new Vector3(0f, steerValue * maxTurn * Time.deltaTime, 0f);
				steeringWheels[i].transform.localEulerAngles = new Vector3(0f, touchAxis.x * maxTurn, 0f);

			}

			// rotate wheels
			for (int i = 0; i < wheelMeshes.Count; i++)
			{
				wheelMeshes[i].transform.Rotate(rb.velocity.magnitude
					* (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) * 5f
					/ (2 * Mathf.PI * .33f), 0f, 0f); // .33 = radius
			}
		}
	}
}