namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class OvershieldPUP : PowerUp
	{
		public GameObject shieldGO;
		public float overshieldDuration = 10f;

		// Start is called before the first frame update
		void Start()
		{
			isPassive = false;
		}

		public override void Init()
		{
			duration = overshieldDuration;
			powerUpGO = shieldGO;
			powerUpType = PowerUpTypes.Overshield;
			// make this the current active powerup
			if (GetComponent<KartV3>())
			{
				PowerUp[] powerUps = GetComponents<PowerUp>();
				foreach (PowerUp pup in powerUps)
				{
					pup.isActive = false;
				}
				isActive = true;
			}

		}

		// Update is called once per frame
		void Update()
		{
			if (!GetComponent<KartV3>() || isExecuted)
			{
				return;
			}

			if (isPassive)
			{
				ExecutePowerUp();
			}
			else if (isActive)
			{
				Debug.Log("Ready to execute");
				/*if (OVRInput.GetDown(OVRInput.RawButton.A))
				{
					ExecutePowerUp();
				}*/
				if (Input.GetButtonDown("Jump"))
				{
					ExecutePowerUp();
				}
			}
		}

		void ExecutePowerUp()
		{
			Debug.Log("Applying Overshield...");
			GameObject os = Instantiate(powerUpGO, transform.position, Quaternion.identity);
			os.transform.parent = transform;
			Destroy(os, overshieldDuration);
			isExecuted = !isExecuted;
		}

		void OnTriggerEnter(Collider other)
		{
			GameObject go = other.gameObject;
			if (go.CompareTag("Player"))
			{
				if (kart.GetComponent<OvershieldPUP>())
				{
					Debug.Log("Already have an OS, destroying this one");
					Destroy(kart.GetComponent<OvershieldPUP>());
				}
				// create the new PowerUp
				PowerUp po = kart.gameObject.AddComponent<OvershieldPUP>();
				// if it is the only PowerUp then it should be active
				PowerUp[] pups = kart.gameObject.GetComponents<PowerUp>();
				po.Init();
				po.powerUpGO = powerUpGO;
				po.kart = kart;
				po.uses = uses;
				po.passiveLifetime = passiveLifetime;
				Debug.Log("Destroying!");
				Destroy(gameObject);
			}
		}
	}
}


