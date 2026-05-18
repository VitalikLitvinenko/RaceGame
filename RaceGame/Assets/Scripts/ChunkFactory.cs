using UnityEngine;

public class ChunkFactory : IChunkFactory
{
    public GameObject Create(GameObject prefab, Transform parent)
    {
        return GameObject.Instantiate(prefab, parent);
    }
}