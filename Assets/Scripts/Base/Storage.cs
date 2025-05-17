using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _maxCapacity;

    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();
    private int _currentAmount = 0;

    public event Action<Dictionary<ResourceType, int>> ResourceChanged;
    public event Action<int> TotalAmountChanged;

    public void Collect(ResourcePiece resource, int amount)
    {
        int spaceLeft = _maxCapacity - _currentAmount;
        int amountToAdd = Mathf.Min(amount, spaceLeft);

        if (_resources.ContainsKey(resource.PeiceType))
        {
            _resources[resource.PeiceType] += amountToAdd;
        }
        else
        {
            _resources.Add(resource.PeiceType, amountToAdd);
        }

        _currentAmount += amountToAdd;
        ResourceChanged?.Invoke(_resources);
        TotalAmountChanged?.Invoke(_currentAmount);
    }

    public bool IsOverflow()
    {
        return _maxCapacity == _currentAmount;
    }
}
