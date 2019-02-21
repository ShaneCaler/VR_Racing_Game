namespace VRTK
{
	using UnityEngine;

	public class Kart_Controller : MonoBehaviour
	{
		public GameObject kart;
		private KartV3 kartScript;

		private void Start()
		{
			kartScript = kart.GetComponent<KartV3>();
			GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);
			GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

			GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
			GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

			GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoCarReset);
		}

		private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
		{
			//Debug.Log("SetTouchAxis called with value of " + e.touchpadAxis);
			kartScript.SetTouchAxis(e.touchpadAxis);
		}

		private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
		{
			kartScript.SetTriggerAxis(e.buttonPressure, e.controllerReference);
		}

		private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
		{
			kartScript.SetTouchAxis(Vector2.zero);
		}

		private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
		{
			kartScript.SetTriggerAxis(0f);
		}

		private void DoCarReset(object sender, ControllerInteractionEventArgs e)
		{
			kartScript.ResetCar();
		}
	}
}