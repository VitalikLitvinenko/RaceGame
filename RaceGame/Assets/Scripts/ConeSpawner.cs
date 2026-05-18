using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConeSpawner : MonoBehaviour
{
    [Header("Cone Settings")]
    public GameObject ConePrefab;
    public int ConeCount = 5;
    public float LaneWidth = 2f;
    public int LaneCount = 2;
    public float ConeSpacing = 1.5f;
    public float SpawnOffsetZ = 2f;

    private List<GameObject> _cones = new List<GameObject>();

    private void Start()
    {
        SpawnCones();
    }

    private void OnEnable()
    {
        if (_cones.Count > 0)
            SpawnCones();
    }

    private void OnDisable()
    {
        ClearCones();
    }

    private void SpawnCones()
    {
        int randomLane = Random.Range(0, LaneCount);
        float laneX = GetLaneX(randomLane);

        for (int i = 0; i < ConeCount; i++)
        {
            Vector3 localPosition = new Vector3(laneX, 0f, SpawnOffsetZ + i * ConeSpacing);

            GameObject cone;
            if (i < _cones.Count)
            {
                cone = _cones[i];
                cone.transform.DOKill();
                cone.transform.localPosition = localPosition;
                cone.transform.localRotation = Quaternion.identity;
                cone.SetActive(true);
            }
            else
            {
                cone = Instantiate(ConePrefab, transform);
                cone.transform.localPosition = localPosition;
                cone.transform.localRotation = Quaternion.identity;
                _cones.Add(cone);
            }
        }
    }

    private void ClearCones()
    {
        foreach (var cone in _cones)
        {
            if (cone != null)
            {
                cone.transform.DOKill();
                cone.SetActive(false);
            }
        }
    }

    private float GetLaneX(int lane)
    {
        float totalWidth = (LaneCount - 1) * LaneWidth;
        return -totalWidth / 2f + lane * LaneWidth;
    }
}