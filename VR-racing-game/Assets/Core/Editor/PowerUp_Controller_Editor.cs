namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.SceneManagement;

	[CustomEditor(typeof(PowerUp_Controller))]
	[CanEditMultipleObjects]
	public class PowerUp_Controller_Editor : Editor
	{
		public PowerUpTypes powerUpTypes;
		SerializedProperty kartProp, speedBoostStrProp, speedBoostDurProp,
			powerUpTypeProp, healthBoostStrProp, overshieldGOProp, overshieldDurProp;

		void OnEnable()
		{
			kartProp = serializedObject.FindProperty("kart");
			speedBoostStrProp = serializedObject.FindProperty("speedBoostStrength");
			speedBoostDurProp = serializedObject.FindProperty("speedBoostDuration");
			powerUpTypeProp = serializedObject.FindProperty("powerUpType");
			healthBoostStrProp = serializedObject.FindProperty("healthBoostStr");
			overshieldGOProp = serializedObject.FindProperty("overshieldGO");
			overshieldDurProp = serializedObject.FindProperty("overshieldDuration");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			PowerUp_Controller _target = (PowerUp_Controller)target;

			bool allowSceneObjects = !EditorUtility.IsPersistent(target);
			_target.kart = (GameObject)EditorGUILayout.ObjectField("Kart Game Object:", _target.kart, typeof(GameObject), allowSceneObjects);

			//EditorGUILayout.PropertyField(kartProp, new GUIContent("Kart Object"));

			powerUpTypes = (PowerUpTypes)EditorGUILayout.EnumPopup("Types: ", powerUpTypes);
			switch (powerUpTypes)
			{
				case PowerUpTypes.SpeedBoost:
					_target.powerUpType = PowerUpTypes.SpeedBoost;
					_target.speedBoostStrength = EditorGUILayout.FloatField("Speed Boost Strength:", _target.speedBoostStrength);
					_target.speedBoostDuration = EditorGUILayout.FloatField("Speed Boost Duration:", _target.speedBoostDuration);
					break;
				case PowerUpTypes.HealthBoost:
					_target.powerUpType = PowerUpTypes.HealthBoost;
					_target.healthBoostStrength = EditorGUILayout.IntField("Health Boost Strength:", _target.healthBoostStrength);
					break;
				case PowerUpTypes.Overshield:
					_target.powerUpType = PowerUpTypes.Overshield;
					_target.overshieldGO = (GameObject)EditorGUILayout.ObjectField("Overshield Game Object:", _target.overshieldGO, typeof(GameObject), false);
					_target.overshieldDuration = EditorGUILayout.FloatField("Overshield Duration:", _target.overshieldDuration);
					break;
				default:
					return;
			}

			if (GUI.changed)
			{
				EditorUtility.SetDirty(_target);
				EditorSceneManager.MarkSceneDirty(_target.gameObject.scene);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}

