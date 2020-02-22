using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject playerActiveChunk;
    public int chunkSize = 128;
    public static MeshGenerator instance;
    public Material mat;
    public int mapSize;
    public List<GameObject> chunkList;
    public Vector2 startChunk;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        mapSize = 8;

        chunkList = new List<GameObject>();
        
        GenerateChunk((int)startChunk.x, (int)startChunk.y);
        if (playerActiveChunk == null)
        {
            playerActiveChunk = chunkList[0];
        }
        GenerateNeighbours();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    GenerateChunk((int)startChunk.x, (int)startChunk.y);
        //    if(playerActiveChunk == null)
        //    {
        //        playerActiveChunk = chunkList[0];
        //    }
        //    GenerateNeighbours();
        //    Debug.Log("w Pressed Generate mesh");
        //}
    }

    public void GenerateChunk(int x, int y)
    {
        if (FileManager.ChunkExists(x, y))
        {
            Debug.Log("loading chunk x y ");
            GameObject newChunk = new GameObject();
            newChunk.name = "Chunk " + x.ToString() + y.ToString();
            newChunk.AddComponent<Chunk>();
            newChunk.GetComponent<Chunk>().BuildChunk(x, y);
            newChunk.GetComponent<MeshRenderer>().material = mat;
            newChunk.GetComponent<Chunk>().cd.arrayPos = new Vector2(x, y);
            newChunk.gameObject.transform.position = new Vector3(x*chunkSize, 0, y* chunkSize);
            chunkList.Add(newChunk);
            FileManager.UnloadChunk(newChunk.GetComponent<Chunk>().cd);
        }
    }

    void GenerateNeighbours()
    {
        Chunk c = playerActiveChunk.GetComponent<Chunk>();
        for (int i = 0; i < playerActiveChunk.GetComponent<Chunk>().cd.neighbors.Length; i++)
        {
            MeshGenerator.instance.GenerateChunk((int)c.cd.neighbors[i].x, (int)c.cd.neighbors[i].y);
        }
    }

}
