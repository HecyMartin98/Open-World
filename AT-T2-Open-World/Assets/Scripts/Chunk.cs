using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    ChunkData cd;
    public Mesh chunkMesh;
    public MeshCollider collider;
    public void BuildChunk(int x, int y)
    {
        if (FileManager.ChunkExists(x, y))
        {
            cd = new ChunkData();
            cd.size = 128;
            CreateMesh();
            SetPos(x, y);
            //CreateJsonFile below
            FileManager.UnloadChunk(this.cd);
        }


    }

    private void Start()
    {
        //cd = new ChunkData();
        //cd.size = 128;
        //CreateMesh();
    }

    void CreateMesh()
    {
        GetComponent<MeshFilter>().mesh = chunkMesh = new Mesh();
        collider = GetComponent<MeshCollider>();
        chunkMesh.name = "Procedural Grid";
        cd.vertices = new Vector3[(cd.size + 1) * (cd.size + 1)];
        Vector2[] uv = new Vector2[cd.vertices.Length];
        Vector4[] tangents = new Vector4[cd.vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, z = 0; z <= cd.size; z++)
        {
            for (int x = 0; x <= cd.size; x++, i++)
            {
                cd.vertices[i] = new Vector3(x, 0 , z);
                uv[i] = new Vector2((float)x / cd.size, (float)z / cd.size);
                tangents[i] = tangent;
            }
        }

        chunkMesh.vertices = cd.vertices;
        chunkMesh.uv = uv;
        chunkMesh.tangents = tangents;
        int[] triangles = new int[cd.size * cd.size * 6];
        for (int ti = 0, vi = 0, z = 0; z < cd.size; z++, vi++)
        {
            for (int x = 0; x < cd.size; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + cd.size + 1;
                triangles[ti + 5] = vi + cd.size + 2;
            }
        }
        chunkMesh.triangles = triangles;
        chunkMesh.RecalculateNormals();
        collider.sharedMesh = chunkMesh;
    }

    public void SetPos(int x, int y)
    {
        cd.pos = new Vector3((x * cd.size), 0, (y * cd.size));
        gameObject.transform.position = cd.pos;
    }

    public void assignChunkData(ChunkData loadedCD)
    {
        this.cd = loadedCD;
        //This creates a brand new mesh, but maybe create OBJ and load the obj file and assign it to the mesh here instead.
        CreateMesh();
    }
}
