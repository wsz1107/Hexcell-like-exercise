using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    public int X { get; private set; }
    public int Y { get { return -X - Z; } }
    public int Z { get; private set; }
    public HexCoordinates(int x,int z)
    {
        X = x;
        Z = z;
    }
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z - x / 2);
    }
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float z = position.z / (HexMetrics.innerRadius * 2f);
        float y = -z;
        float offset = position.x / (HexMetrics.outerRadius * 3f);
        z -= offset;
        y -= offset;
        int iZ = Mathf.RoundToInt(z);
        int iY = Mathf.RoundToInt(y);
        int iX = Mathf.RoundToInt(-z - y);
        if (iX + iY + iZ != 0)
        {
            float dZ = Mathf.Abs(z - iZ);
            float dY = Mathf.Abs(y - iY);
            float dX = Mathf.Abs(-y - z - iX);
            if (dZ > dY && dZ > dX)
            {
                iZ = -iX - iY;
            }
            else if (dX > dY)
            {
                iX = -iZ - iY;
            }
        }
        return new HexCoordinates(iX, iZ);
    }
    public override string ToString()
    {
        return "(" + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")";
    }
    public string ToStringOnSeperateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}
public static class HexMetrics
{
    public const float outerRadius =10f;
    public const float innerRadius = outerRadius * 0.866025404f;
    public const float factor = 0.9f;
    public const float frameWidth = 0.05f;
    public static Color[] colors;
    public static Vector3[] corners =
    {
        new Vector3(outerRadius,0f, 0f),
        new Vector3(0.5f * outerRadius,0.0f,-innerRadius),
        new Vector3(-0.5f * outerRadius,0.0f,-innerRadius),
        new Vector3(-outerRadius,0f, 0f),
        new Vector3(-0.5f * outerRadius,0.0f,innerRadius),
        new Vector3(0.5f * outerRadius,0.0f,innerRadius),
        new Vector3(outerRadius,0f, 0f)
    };
}
