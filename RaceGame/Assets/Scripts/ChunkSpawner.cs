using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : IChunkSpawner
{
    private ChunkPool _pool;
    private List<Transform> _activeChunks;
    private Transform _parent;
    private float _blockLength;
    private int _initialBlockCount;
    private Transform _cameraTransform;

    public ChunkSpawner(ChunkPool pool, List<Transform> activeChunks, Transform parent,
        Transform cameraTransform, float blockLength, int initialBlockCount)
    {
        _pool = pool;
        _activeChunks = activeChunks;
        _parent = parent;
        _cameraTransform = cameraTransform;
        _blockLength = blockLength;
        _initialBlockCount = initialBlockCount;
    }

    public void SpawnInitialChunks()
    {
        float nextSpawnPositionZ = _cameraTransform.position.z;
        for (int i = 0; i < _initialBlockCount; i++)
        {
            SpawnChunk(nextSpawnPositionZ);
            nextSpawnPositionZ += _blockLength;
        }
    }

    public void SpawnChunk(float zPosition)
    {
        Vector3 spawnPosition = new Vector3(_parent.position.x, _parent.position.y, zPosition);
        GameObject newChunk = _pool.Get(spawnPosition);
        _activeChunks.Add(newChunk.transform);
    }
}