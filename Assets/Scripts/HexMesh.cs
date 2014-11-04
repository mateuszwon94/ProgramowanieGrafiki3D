using UnityEngine;
using System.Collections;

public class HexMesh : MonoBehaviour
{
	public readonly float heightOverTerrain = 0.2f;
	
	void Awake ()
	{
		Ray ray; //chwyta również budynki i propsy, do modyfikacji
		RaycastHit hitInfo;
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		
		Vector3[] vertices = mesh.vertices;
		
		ray = new Ray(transform.position + Vector3.up * 999, Vector3.down);
		if (Physics.Raycast(ray, out hitInfo, 9999))
		{
			Vector3 temp = transform.position;
			temp.Set(temp.x, hitInfo.point.y, temp.z);
			transform.position = temp;
		}
		
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			// also works on scaled hexes
			ray = new Ray(transform.position + new Vector3 (vertices[i].x * transform.lossyScale.x , vertices[i].y * transform.lossyScale.y, vertices[i].z * transform.lossyScale.z) + Vector3.up * 999, Vector3.down);
			if (Physics.Raycast(ray, out hitInfo, 9999))
			{
				vertices[i].Set(vertices[i].x, (hitInfo.point.y - transform.position.y + heightOverTerrain), vertices[i].z);
			}
		}
		
		mesh.vertices = vertices;
	}
}
