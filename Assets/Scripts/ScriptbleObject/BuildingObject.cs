using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new build", menuName = "Builds", order = 51)]
public class BuildingObject : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _goldCost;
    [SerializeField] private int _ironCost;
    [SerializeField] private int _cupperCost;

    public GameObject Prefab => _prefab;

    public Dictionary<ResourceType, int> ReturnCost()
    {
        Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>
        {
            { ResourceType.Gold, _goldCost },
            { ResourceType.Iron, _ironCost },
            { ResourceType.Cupper, _cupperCost }
        };

        return cost;
    }
}
