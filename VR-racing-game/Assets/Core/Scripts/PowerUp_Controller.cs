namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[System.Serializable]
	public enum PowerUpTypes
	{
		None,
		SpeedBoost,
		HealthBoost,
		Overshield,
		Missile,
		Trap,
		Slime, // Distort an enemies vision
		EMP,
		InvisibilityShroud,
		GrapplingHook
	}

	[System.Serializable]
	public class PowerUp_Controller : MonoBehaviour
	{
		public GameObject kart;
		[HideInInspector]
		public PowerUpTypes powerUpType;
		[HideInInspector]
		public float speedBoostStrength = 5000f;
		[HideInInspector]
		public float speedBoostDuration = 5f;
		public int healthBoostStrength = 25;
		public GameObject overshieldGO;
		public float overshieldDuration = 10f;

		private KartV3 kartScript;

		void OnCollisionEnter(Collision collision)
		{
			switch (powerUpType)
			{
				case PowerUpTypes.SpeedBoost:
					Debug.Log("Speed Boost Triggered");
					StartCoroutine(AlternateStrength());
					break;
				case PowerUpTypes.HealthBoost:
					Debug.Log("Health Boost Triggered");
					kartScript.SetHealthPoints(healthBoostStrength);
					break;
				case PowerUpTypes.Overshield:
					Debug.Log("Overshield Triggered");
					kartScript.ApplyOvershield(overshieldGO, overshieldDuration);
					break;
				default:
					Debug.Log("Oops! In default, returning.");
					return;
			}
		}

		IEnumerator AlternateStrength()
		{
			kartScript.SetTorqueStrength(speedBoostStrength);
			yield return new WaitForSeconds(speedBoostDuration);
			kartScript.SetTorqueStrength(-speedBoostStrength);
		}

		// Start is called before the first frame update
		void Start()
		{
			kartScript = kart.GetComponent<KartV3>();
		}

		// Update is called once per frame
		void Update()
		{
			Debug.Log("Powerup type is " + powerUpType);
		}
	}
}

