using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileManager 
{
    public static ChunkData LoadChunk(int x, int y)
    {
        ChunkData newChunk;
        string jsonFilesToLoad = File.ReadAllText
            (Application.dataPath + "/StreamingAssets/ChunkData" + x.ToString() + y.ToString() + ".json");
        newChunk = JsonUtility.FromJson<ChunkData>(jsonFilesToLoad);
        return newChunk;
    }

    public static void UnloadChunk(ChunkData cd)
    {
        string json = JsonUtility.ToJson(cd);
        File.WriteAllText
            (Application.dataPath + "/StreamingAssets/ChunkData" + cd.arrayPos.x.ToString() + cd.arrayPos.y.ToString() + ".json", json);
    }

    public static bool ChunkExists(int x, int y)
    {
        string filePath = (Application.dataPath + "/StreamingAssets/ChunkData" + x.ToString() + y.ToString() + ".json");
        bool exists = File.Exists(filePath);
        return exists;
    }
}
