using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _holdDistance = 1.5f;
    [SerializeField] private int _pickAmount = 1;
    [SerializeField] private float _extractTime = 2f;

    private List<ResourcePiece> _resources = new List<ResourcePiece>();
    private ResourcePiece _currentObject;
    private WaitForSeconds _delay;

    public event Action<ResourcePiece, int> Arrived;

    private void Awake()
    {
        _delay = new WaitForSeconds(_extractTime);
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
        _currentObject.gameObject.SetActive(true);
    }

    private ResourcePiece GetResource(ResourceNode resourceNode)
    {
        ResourcePiece newResource = resourceNode.Extract(_pickAmount);

        if (_resources.Count > 0)
        {
            foreach(var resource in _resources)
            {
                if (resource.PeiceType == newResource.PeiceType)
                {
                    return resource; 
                }
            }
        }

        return CreateResourse(newResource); 
    }

    private ResourcePiece CreateResourse(ResourcePiece newResource)
    {
        ResourcePiece currentResource = Instantiate(newResource);
        currentResource.transform.SetParent(transform);
        currentResource.transform.localPosition = new Vector3(0f, 0f, _holdDistance);
        _resources.Add(currentResource);

        return currentResource;
    }

    private void DeactivateCurrentResource()
    {
        if (_currentObject != null)
        {
            _currentObject.gameObject.SetActive(false);
        }
    }
}