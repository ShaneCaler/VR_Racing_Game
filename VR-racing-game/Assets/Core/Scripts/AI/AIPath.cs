using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPath : MonoBehaviour
{
	public Color lineColor;
	public bool alwaysShowGizmos = false;
	private List<Transform> nodes = new List<Transform>();


	private void OnDrawGizmosSelected()
	{
		if (!alwaysShowGizmos)
		{
			DrawGizmos();
		}
	}

	private void OnDrawGizmos()
	{
		if (alwaysShowGizmos)
		{
			DrawGizmos();
		}
	}

	private void DrawGizmos()
	{
		Gizmos.color = lineColor;

		Transform[] pathTransforms = GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();

		for (int i = 0; i < pathTransforms.Length; i++)
		{
			if (pathTransforms[i] != transform)
				nodes.Add(pathTransforms[i]);
		}

		int nodeCount = nodes.Count;
		for (int i = 0; i < nodeCount; i++)
		{
			Vector3 currNode = nodes[i].position;
			Vector3 prevNode = nodes[(nodeCount - 1 + i) % nodeCount].position;
			Gizmos.DrawLine(prevNode, currNode);
			Gizmos.DrawWireSphere(currNode, 0.3f);
		}
	}
}
