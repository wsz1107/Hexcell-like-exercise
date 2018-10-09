using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FrameMesh : MonoBehaviour
{
    Mesh frameMesh;
    HexGrid hexGrid;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = frameMesh = new Mesh();
        frameMesh.name = "FrameMesh";
        hexGrid = GetComponentInParent<HexGrid>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }
    public void FrameHighLight(HexCell[] cells)
    {
        frameMesh.Clear();
        triangles.Clear();
        vertices.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            FrameHighLight(cells[i]);
        }
        frameMesh.vertices = vertices.ToArray();
        frameMesh.triangles = triangles.ToArray();
        frameMesh.colors = colors.ToArray();
        frameMesh.RecalculateNormals();
    }
    public void FrameHighLight(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddFrameTriangles(center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1], center + HexMetrics.corners[i] * HexMetrics.factor);
            AddFrameTriangles(center + HexMetrics.corners[i] * HexMetrics.factor, center + HexMetrics.corners[i + 1], center + HexMetrics.corners[i + 1] * HexMetrics.factor);
            AddFrameColor(cell.frameColor);
        }
    }
    public void ClearAllFrame(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddFrameTriangles(center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1], center + HexMetrics.corners[i] * HexMetrics.factor);
            AddFrameTriangles(center + HexMetrics.corners[i] * HexMetrics.factor, center + HexMetrics.corners[i + 1], center + HexMetrics.corners[i + 1] * HexMetrics.factor);
            //cell.HexCellFrameColorIndex = 0;
            //AddFrameColor(hexGrid.colors[cell.HexCellFrameColorIndex]);
            AddFrameColor(hexGrid.defaultFrameColor);
        }
    }
    void AddFrameTriangles(Vector3 v1,Vector3 v2,Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
    void AddFrameColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
    public void ClearAllFrame(HexCell[] cells)
    {
        frameMesh.Clear();
        triangles.Clear();
        vertices.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            ClearAllFrame(cells[i]);
        }
        frameMesh.vertices = vertices.ToArray();
        frameMesh.triangles = triangles.ToArray();
        frameMesh.colors = colors.ToArray();
        frameMesh.RecalculateNormals();
    }
}
