using System.Collections.Generic;
using UnityEngine;

public class ChunkMover : IChunkMover
{
    private List<Transform> _activeChunks;

    public ChunkMover(List<Transform> activeChunks)
    {
        _activeChunks = activeChunks;
    }

    public void MoveBlocks(float moveSpeed)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 moveOffset = new Vector3(0, 0, -moveDistance);
        foreach (var activeChunk in _activeChunks)
        {
            activeChunk.position += moveOffset;
        }
    }
}