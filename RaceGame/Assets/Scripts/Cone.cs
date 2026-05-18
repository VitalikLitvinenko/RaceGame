using UnityEngine;
using DG.Tweening;

public class Cone : MonoBehaviour
{
    [Header("Fly Settings")]
    public float FlyDistance = 5f;
    public float FlyHeight = 2f;
    public float FlyDuration = 0.5f;

    private bool _isHit = false;
    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.DOKill();
        _isHit = false;
        transform.localPosition = _initialLocalPosition;
    }

    private void OnDisable()
    {
        transform.DOKill();
        _isHit = false;
        transform.localPosition = _initialLocalPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHit) return;
        if (!other.CompareTag("Player")) return;

        _isHit = true;
        FlyBack(other.transform.forward);
    }

    public void FlyBack(Vector3 carForward)
    {
        Vector3 flyDirection = -carForward;
        Vector3 targetPosition = transform.position + flyDirection * FlyDistance;
        targetPosition.y += FlyHeight;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPosition, FlyDuration).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMoveY(targetPosition.y - FlyHeight, FlyDuration).SetEase(Ease.InQuad));
        sequence.OnComplete(() =>
        {
            if (gameObject != null)
                gameObject.SetActive(false);
        });
    }
}