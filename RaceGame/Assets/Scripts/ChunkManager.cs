using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform CameraTransform;
    public List<GameObject> Chunks = new List<GameObject>();

    public int InitialBlockCount = 8;
    public float BlockLength = 10;

    public float StartMoveSpeed = 10;
    public float MaxSpeed = 30f;
    public float SpeedIncreasePerSecond = 0.4f;

    public float recycleDistanceBehindCamera = 15;

    public float CurrentSpeed => _speedCalculator.CurrentSpeed;

    private List<Transform> _activeChunks = new List<Transform>();

    private ChunkPool _pool;
    private IChunkFactory _factory;
    private IChunkSpawner _spawner;
    private IChunkMover _mover;
    private IChunkRecycler _recycler;
    private ISpeedCalculator _speedCalculator;

    private void Awake()
    {
        _factory = new ChunkFactory();
        _pool = new ChunkPool(Chunks, transform, InitialBlockCount + 2, _factory);
        _spawner = new ChunkSpawner(_pool, _activeChunks, transform, CameraTransform, BlockLength, InitialBlockCount);
        _mover = new ChunkMover(_activeChunks);
        _recycler = new ChunkRecycler(_activeChunks, _pool, _spawner, CameraTransform, recycleDistanceBehindCamera, BlockLength);
        _speedCalculator = new SpeedCalculator(StartMoveSpeed, MaxSpeed, SpeedIncreasePerSecond);

        _spawner.SpawnInitialChunks();
    }

    private void Update()
    {
        _speedCalculator.RecalculateSpeed();
        _mover.MoveBlocks(_speedCalculator.CurrentSpeed);
        _recycler.RecycleBlockPassedCamera();
    }
}