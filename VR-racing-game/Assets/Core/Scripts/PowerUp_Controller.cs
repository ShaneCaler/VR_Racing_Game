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

	public class PowerUp_Controller : MonoBehaviour
	{
		public KartV3 kart;
		public PowerUpTypes powerUpType;
		public float speedBoostStrength = 5000f;
		public float speedBoostDuration = 5f;
		public float healthBoostStrength = 25f;
		public GameObject overshieldGO;
		public float overshieldDuration = 10f;

		public bool speedBoostToggle, healthBoostToggle, overshieldToggle;
		public GameObject pickupEffect;

		void OnTriggerEnter(Collider other)
		{
			Debug.Log("collided with " + other);
			if (other.CompareTag("Player"))
			{
				Pickup(other);
			}

		}

		void Pickup(Collider player)
		{
			// Spawn particle effect
			Instantiate(pickupEffect, transform.position, transform.rotation);

			// Apply effect
			switch (powerUpType)
			{
				case PowerUpTypes.SpeedBoost:
					Debug.Log("Speed Boost Triggered");
					kart.ApplySpeedBoost(speedBoostStrength, speedBoostDuration);
					break;
				case PowerUpTypes.HealthBoost:
					Debug.Log("Health Boost Triggered");
					kart.SetHealthPoints(healthBoostStrength);
					break;
				case PowerUpTypes.Overshield:
					Debug.Log("Overshield Triggered");
					kart.ApplyOvershield(overshieldGO, overshieldDuration);
					break;
				default:
					Debug.Log("Oops! In default, returning.");
					return;
			}

			Destroy(gameObject);
		}

		// Start is called before the first frame update
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			// Debug.Log("Powerup type is " + powerUpType);
		}
	}
}

