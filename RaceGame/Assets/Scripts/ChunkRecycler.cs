using System.Collections.Generic;
using UnityEngine;

public class ChunkRecycler : IChunkRecycler
{
    private List<Transform> _activeChunks;
    private ChunkPool _pool;
    private IChunkSpawner _spawner;
    private Transform _cameraTransform;
    private float _recycleDistanceBehindCamera;
    private float _blockLength;

    public ChunkRecycler(List<Transform> activeChunks, ChunkPool pool, IChunkSpawner spawner,
        Transform cameraTransform, float recycleDistanceBehindCamera, float blockLength)
    {
        _activeChunks = activeChunks;
        _pool = pool;
        _spawner = spawner;
        _cameraTransform = cameraTransform;
        _recycleDistanceBehindCamera = recycleDistanceBehindCamera;
        _blockLength = blockLength;
    }

    public void RecycleBlockPassedCamera()
    {
        float recycleThreshold = _cameraTransform.position.z - _recycleDistanceBehindCamera;
        while (_activeChunks.Count > 0)
        {
            Transform oldestBlock = _activeChunks[0];
            if (oldestBlock.position.z >= recycleThreshold)
            {
                return;
            }

            _activeChunks.RemoveAt(0);
            float frontZ = _activeChunks.Count == 0 ? oldestBlock.position.z : GetFrontPositionZ();
            float nextZ = frontZ + _blockLength;

            _pool.Return(oldestBlock.gameObject);
            _spawner.SpawnChunk(nextZ);
        }
    }

    private float GetFrontPositionZ()
    {
        float returnValue = float.MinValue;
        foreach (var activeChunk in _activeChunks)
        {
            if (activeChunk.position.z > returnValue)
            {
                returnValue = activeChunk.position.z;
            }
        }
        return returnValue;
    }
}