namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.SceneManagement;

	[CustomEditor(typeof(PowerUp_Controller))]
	public class PowerUp_Controller_Editor : Editor
	{
		public PowerUpTypes powerUpTypes;
		[SerializeField]
		private bool speedBoostToggle;
		private SerializedObject _object;
		private SerializedProperty kartProp, destroyDelayProp, speedBoostStrProp, speedBoostDurProp,
			 powerUpTypeProp, healthBoostStrProp, overshieldGOProp, overshieldDurProp;

		[SerializeField]
		private PowerUp_Controller test;
		 void OnEnable()
		{
			_object = new SerializedObject(target);
			kartProp = _object.FindProperty("kart");
			destroyDelayProp = _object.FindProperty("destroyDelay");
			speedBoostStrProp = _object.FindProperty("speedBoostStrength");
			speedBoostDurProp = _object.FindProperty("speedBoostDuration");
			powerUpTypeProp = _object.FindProperty("powerUpType");
			healthBoostStrProp = _object.FindProperty("healthBoostStrength");
			overshieldGOProp = _object.FindProperty("overshieldGO");
			overshieldDurProp = _object.FindProperty("overshieldDuration");
			test = (PowerUp_Controller)target;
		}

		public override void OnInspectorGUI()
		{
			_object.Update();
			PowerUp_Controller _target = (PowerUp_Controller)target;

			EditorGUILayout.PropertyField(destroyDelayProp);
			_target.speedBoostToggle = EditorGUILayout.Toggle("Speed Boost", _target.speedBoostToggle);
			_target.healthBoostToggle = EditorGUILayout.Toggle("Health Boost", _target.healthBoostToggle);
			_target.overshieldToggle = EditorGUILayout.Toggle("Overshield", _target.overshieldToggle);

			if (_target.speedBoostToggle)
			{
				_target.powerUpType = PowerUpTypes.SpeedBoost;
				EditorGUILayout.PropertyField(speedBoostStrProp);
				EditorGUILayout.PropertyField(speedBoostDurProp);
			}
			else if (_target.healthBoostToggle)
			{
				_target.powerUpType = PowerUpTypes.HealthBoost;
				EditorGUILayout.PropertyField(healthBoostStrProp);
			}
			else if (_target.overshieldToggle)
			{
				_target.powerUpType = PowerUpTypes.Overshield;
				EditorGUILayout.ObjectField(overshieldGOProp);
				EditorGUILayout.PropertyField(overshieldDurProp);
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
				EditorSceneManager.MarkAllScenesDirty();
			}


			_object.ApplyModifiedProperties();
		}

		//void OnFocus()
		//{
		//	if (EditorPrefs.HasKey("speedBoostToggle"))
		//		speedBoostToggle = EditorPrefs.GetBool("speedBoostToggle");
		//}

		//void OnLostFocus()
		//{
		//	EditorPrefs.SetBool("speedBoostToggle", speedBoostToggle);
		//}

		//void OnDestroy()
		//{
		//	EditorPrefs.SetBool("speedBoostToggle", speedBoostToggle);
		//}
	}
}


