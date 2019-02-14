namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class HealthBoostPUP : PowerUp
	{
		public float healthBoostStrength = 50f;

		// Start is called before the first frame update
		void Start()
		{
			isActive = false;
		}

		public override void Init()
		{
			strength = healthBoostStrength;
			powerUpType = PowerUpTypes.HealthBoost;
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
			kart.healthPoints += strength;

		}

		void OnTriggerEnter(Collider other)
		{
			GameObject go = other.gameObject;
			if (go.CompareTag("Player"))
			{
				if (go.GetComponent<HealthBoostPUP>())
				{
					Destroy(go.GetComponent<HealthBoostPUP>());
				}
				// create the new PowerUp
				PowerUp po = go.AddComponent<HealthBoostPUP>();

				po.Init();

				Destroy(gameObject);
			}
		}
	}
}


