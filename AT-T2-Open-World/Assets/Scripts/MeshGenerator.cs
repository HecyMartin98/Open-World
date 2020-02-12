using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public int mapSize;
    public List<GameObject> chunkList;
    // Start is called before the first frame update
    void Start()
    {
        mapSize = 3;

        chunkList = new List<GameObject>();
        //Generate Meshes here
        GameObject newChunk = new GameObject();
        //for (int i = 0; i < mapSize; ++i)
        //{
        //    for (int j = 0; j < mapSize; ++j)
        //    {
        //        if (!FileManager.ChunkExists(i, j))
        //        {
        //            newChunk = new GameObject();
        //            newChunk.AddComponent<Chunk>();
        //            newChunk.GetComponent<Chunk>().BuildChunk(i, j);
        //            chunkList.Add(newChunk);
        //        }
        //        else
        //        {
        ChunkData newCd = FileManager.LoadChunk(0, 0);
        newChunk = new GameObject();
        newChunk.AddComponent<Chunk>();
        newChunk.GetComponent<Chunk>().assignChunkData(newCd);
        //        }
        //    }
        //}

        newChunk.AddComponent<Chunk>();
        newChunk.GetComponent<Chunk>().BuildChunk(0, 0);
        chunkList.Add(newChunk);
    }
}
