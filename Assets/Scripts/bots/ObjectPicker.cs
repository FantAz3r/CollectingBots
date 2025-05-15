using System;
using System.Collections;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _holdDistance = 1.5f;
    [SerializeField] private int _pickAmount = 1;
    [SerializeField] private float _extractTime = 2f;
    [SerializeField] private ResourcePiece[] _resources;

    private ResourcePiece _currentObject;
    private WaitForSeconds _delay;

    public event Action<ResourcePiece, int> Arrived;

    private void Awake()
    {
        _delay = new WaitForSeconds(_extractTime);
        DeactivateAllResources();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Storage>(out _))
        {
            if (_currentObject != null)
            {
                Arrived?.Invoke(_currentObject, _pickAmount);
                DeactivateCurrentResource();
                _currentObject = null;
            }
        }
    }

    public IEnumerator PickUp(ResourceNode resourceNode)
    {
        yield return _delay;
        _currentObject = GetResource(resourceNode);
        _currentObject.PickUp(transform, _holdDistance);
    }

    private void DeactivateAllResources()
    {
        foreach (var resource in _resources)
        {
            resource.gameObject.SetActive(false);
        }
    }

    private ResourcePiece GetResource(ResourceNode resourceNode)
    {
        _currentObject = resourceNode.Extract(_pickAmount);

        for (int i = 0; i < _resources.Length; i++)
        {
            if (_resources[i].PeiceType == _currentObject.PeiceType)
            {
                _resources[i].gameObject.SetActive(true);

                return _resources[i];
            }
        }

        return null;
    }

    private void DeactivateCurrentResource()
    {
        if (_currentObject != null)
        {
            _currentObject.gameObject.SetActive(false);
        }
    }
}