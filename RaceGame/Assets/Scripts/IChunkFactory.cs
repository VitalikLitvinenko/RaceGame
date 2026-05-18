using UnityEngine;

public interface IChunkFactory
{
    GameObject Create(GameObject prefab, Transform parent);
}