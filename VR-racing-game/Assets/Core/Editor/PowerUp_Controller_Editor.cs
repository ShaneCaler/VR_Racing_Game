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
		private SerializedProperty kartProp, speedBoostStrProp, speedBoostDurProp,
			 powerUpTypeProp, healthBoostStrProp, overshieldGOProp, overshieldDurProp;

		[SerializeField]
		private PowerUp_Controller test;
		 void OnEnable()
		{
			_object = new SerializedObject(target);
			kartProp = _object.FindProperty("kart");
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

			//powerUpTypes = (PowerUpTypes)EditorGUILayout.EnumPopup("Types: ", powerUpTypes);

			/* switch (powerUpTypes)
		   {
			   case PowerUpTypes.SpeedBoost:
				   EditorGUILayout.PropertyField(powerUpTypeProp);
				   EditorGUILayout.PropertyField(speedBoostStrProp);
				   EditorGUILayout.PropertyField(speedBoostDurProp);
				   //_target.powerUpType = PowerUpTypes.SpeedBoost;
				   //_target.speedBoostStrength = EditorGUILayout.FloatField("Speed Boost Strength:", _target.speedBoostStrength);
				   //_target.speedBoostDuration = EditorGUILayout.FloatField("Speed Boost Duration:", _target.speedBoostDuration);
				   break;
			   case PowerUpTypes.HealthBoost:
				   //_target.powerUpType = PowerUpTypes.HealthBoost;
				   //_target.healthBoostStrength = EditorGUILayout.IntField("Health Boost Strength:", _target.healthBoostStrength);
				   break;
			   case PowerUpTypes.Overshield:
				   //_target.powerUpType = PowerUpTypes.Overshield;
				   //_target.overshieldGO = (GameObject)EditorGUILayout.ObjectField("Overshield Game Object:", _target.overshieldGO, typeof(GameObject), false);
				   //_target.overshieldDuration = EditorGUILayout.FloatField("Overshield Duration:", _target.overshieldDuration);
				   break;
			   default:
				   return;
		   } */

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
				EditorSceneManager.MarkAllScenesDirty();
			}


			_object.ApplyModifiedProperties();
		}

		void OnFocus()
		{
			if (EditorPrefs.HasKey("speedBoostToggle"))
				speedBoostToggle = EditorPrefs.GetBool("speedBoostToggle");
		}

		void OnLostFocus()
		{
			EditorPrefs.SetBool("speedBoostToggle", speedBoostToggle);
		}

		void OnDestroy()
		{
			EditorPrefs.SetBool("speedBoostToggle", speedBoostToggle);
		}
	}
}


