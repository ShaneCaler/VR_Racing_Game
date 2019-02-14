namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SpeedBoostPUP : PowerUp
	{
		public float speedBoostStrength = 5000f;
		public float speedBoostDuration = 5f;

		// Start is called before the first frame update
		void Start()
		{
			isActive = false;
		}

		public override void Init()
		{
			strength = speedBoostStrength;
			duration = speedBoostDuration;
			powerUpType = PowerUpTypes.SpeedBoost;

			isPassive = true;
			Destroy(this, passiveLifetime);
		}

		// Update is called once per frame
		void Update()
		{
			if (isPassive)
			{
				ExecutePowerUp();
			}
			else if (isActive)
			{
				if (OVRInput.GetDown(OVRInput.RawButton.A))
				{
					ExecutePowerUp();
				}
			}
		}

		void ExecutePowerUp()
		{
			ApplySpeedBoost(strength, duration);
		}

		public void ApplySpeedBoost(float str, float dur)
		{
			StartCoroutine(HandleSpeedBoost(str, dur));
		}

		IEnumerator HandleSpeedBoost(float str, float dur)
		{
			kart.torqueStrength += str;
			yield return new WaitForSeconds(dur);
			kart.torqueStrength -= str;
		}

		void OnTriggerEnter(Collider other)
		{
			GameObject go = other.gameObject;
			if (go.CompareTag("Player"))
			{
				if (go.GetComponent<SpeedBoostPUP>())
				{
					Destroy(go.GetComponent<SpeedBoostPUP>());
				}
				// create the new PowerUp
				PowerUp po = go.AddComponent<SpeedBoostPUP>();

				po.Init();

				Destroy(gameObject);
			}
		}
	}
}


