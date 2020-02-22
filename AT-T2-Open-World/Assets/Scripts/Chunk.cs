using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    public ChunkData cd;
    public Mesh chunkMesh;
    public MeshCollider collider;
    public void BuildChunk(int x, int y)
    {
        if (FileManager.ChunkExists(x, y))
        {
            cd = FileManager.LoadChunk(x, y);
            LoadMesh();

        }
        else
        {
            cd = new ChunkData();
        cd.size = 128;
        CreateMesh();
        SetPos(x, y);
        GetNeighborsOfChunk(x, y);
        FileManager.UnloadChunk(this.cd);
        }

    }

    void LoadMesh()
    {
        Debug.Log("loading mesh from file");
        gameObject.transform.position = cd.pos;
        if(cd!= null)
        {
            collider = GetComponent<MeshCollider>();
            GetComponent<MeshFilter>().mesh = chunkMesh = new Mesh();
            chunkMesh.vertices = cd.vertices;
            chunkMesh.uv = cd.uv;
            chunkMesh.tangents = cd.tangents;
            chunkMesh.triangles = cd.triangles;
            chunkMesh.colors = cd.colours;
            chunkMesh.RecalculateNormals();
            collider.sharedMesh = chunkMesh;
        }
        else
        {
            Debug.Log("Chunk data is null");
        }
        MeshGenerator.instance.chunkList.Add(gameObject);
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


    private void GetNeighborsOfChunk(int x, int y)
    {
        
        List<Vector2> newNeighbors = new List<Vector2>();
        for (int xx = -1; xx <= 1; xx++)
        {
            for (int yy = -1; yy <= 1; yy++)
            {
                if (xx == 0 && yy == 0)
                {
                    continue; // You are not neighbor to yourself
                }
                //if (!CONSIDER_CORNERS && Mathf.Abs(xx) + Mathf.Abs(yy) > 1)
                //{
                //    continue;
                //}
                if (isOnMap(x + xx, y + yy))
                {
                    newNeighbors.Add(new Vector2(x + xx, y + yy));
                }
            }
        }
        cd.neighbors = new Vector2[newNeighbors.Count];
        cd.neighbors = newNeighbors.ToArray();
    }

    

    public bool isOnMap(int x, int y)
    {
        int size = MeshGenerator.instance.mapSize;
        return x >= 0 && y >= 0 && x < size && y < size;
    }
    
}
