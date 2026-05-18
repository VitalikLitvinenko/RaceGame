using UnityEngine;

public class ChunkActivator : MonoBehaviour
{
    private Cone[] _cones;

    private void Awake()
    {
        _cones = GetComponentsInChildren<Cone>(true);
    }

    private void OnEnable()
    {
        foreach (var cone in _cones)
        {
            cone.gameObject.SetActive(true);
        }
    }
}