using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _maxCapacity;

    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();
    private int _currentAmount = 0;
    private int _startAmount = 0;

    public event Action<Dictionary<ResourceType, int>> ResourceChanged;
    public event Action ResourceAdded;
    public event Action<int> TotalAmountChanged;

    private void Start()
    {
        _resources.Add(ResourceType.Gold, _startAmount);
        _resources.Add(ResourceType.Iron, _startAmount);
        _resources.Add(ResourceType.Cupper, _startAmount);
    }

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
        ResourceAdded?.Invoke();
        ViewActions();
    }

    public bool IsEnoughResource(Dictionary<ResourceType, int> cost)
    {
        foreach (var pair in cost)
        {
            if (_resources.ContainsKey(pair.Key) == false || _resources[pair.Key] < pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    public void SpendResource(Dictionary<ResourceType, int> cost)
    {
        foreach (var pair in cost)
        {
            _resources[pair.Key] -= pair.Value;
            _currentAmount -= pair.Value;
        }

        ViewActions();
    }

    public bool IsOverflow()
    {
        return _maxCapacity <= _currentAmount;
    }

    private void ViewActions()
    {
        ResourceAdded?.Invoke();
        ResourceChanged?.Invoke(_resources);
    }
}
