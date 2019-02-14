namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class PowerUp: MonoBehaviour
	{
		public KartV3 kart;
		[HideInInspector]
		public PowerUpTypes powerUpType;
		[HideInInspector]
		public GameObject powerUpGO;
		[HideInInspector]
		public float duration;
		[HideInInspector]
		public float strength;
		public int uses;
		[HideInInspector]
		public bool isActive = false;
		[HideInInspector]
		public bool isPassive = false;
		[HideInInspector]
		public bool isExecuted = false;
		public float passiveLifetime = 10f;

		public virtual void Init()
		{
			if (isPassive)
			{
				Destroy(this, passiveLifetime);
			}
			else
			{
				isActive = true;
			}
		}

		public void Deactivate()
		{
			isActive = false;
		}

		
	}
}

