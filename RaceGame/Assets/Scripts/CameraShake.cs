using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Shake Settings")]
    public float ShakeDuration = 0.3f;
    public float ShakeStrength = 0.3f;
    public int ShakeVibrato = 10;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake()
    {
        transform.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato);
    }
}