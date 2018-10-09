using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    MeshCollider meshCollider;
    List<Vector3> hexVertices;
    List<int> hexTriangles;
    List<Color> hexColors;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "HexMesh";
        hexVertices = new List<Vector3>();
        hexTriangles = new List<int>();
        hexColors = new List<Color>();
    }
    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        hexTriangles.Clear();
        hexVertices.Clear();
        hexColors.Clear();
        for(int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = hexVertices.ToArray();
        hexMesh.triangles = hexTriangles.ToArray();
        hexMesh.colors = hexColors.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }
    public void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for(int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i] * HexMetrics.factor, center + HexMetrics.corners[i + 1] * HexMetrics.factor);
            AddTriangleColor(cell.color);
        }
    }
    void AddTriangle(Vector3 v1,Vector3 v2,Vector3 v3)
    {
        int vertexIndex = hexVertices.Count;
        hexVertices.Add(v1);
        hexVertices.Add(v2);
        hexVertices.Add(v3);
        hexTriangles.Add(vertexIndex);
        hexTriangles.Add(vertexIndex+1);
        hexTriangles.Add(vertexIndex+2);
    }
    void AddTriangleColor(Color color)
    {
        hexColors.Add(color);
        hexColors.Add(color);
        hexColors.Add(color);
    }
}
