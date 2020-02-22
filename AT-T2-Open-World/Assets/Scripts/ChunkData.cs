using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public int id;
    public int size;
    public int[] triangles;
    public Color[] colours;

    public Vector3[] vertices;
    public Vector3 pos;
    public Vector2 arrayPos;
    public Vector2[] neighbors;
    public Vector2[] uv;
    public Vector4[] tangents;
    public Vector4 tangent;
}
