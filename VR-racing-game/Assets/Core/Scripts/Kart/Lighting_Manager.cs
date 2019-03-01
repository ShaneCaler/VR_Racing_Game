using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_Manager : MonoBehaviour
{
	public List<Light> headLights;
	public List<Light> brakeLights;

	// enable when entering dark spaces - no user input
	public virtual void ToggleHeadLights()
	{
		foreach(Light light in headLights)
		{
			light.intensity = light.intensity == 0 ? 2 : 0;
		}
	}

	// enable only when braking
	public virtual void ToggleBrakeLights()
	{
		foreach(Light light in brakeLights)
		{
			light.intensity = light.intensity == 0 ? 2 : 0;
		}
	}
}
