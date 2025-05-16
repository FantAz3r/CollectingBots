using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private List<ResourceNode> _buzyNodes = new List<ResourceNode>();
    private List<ResourceNode> _nodes = new List<ResourceNode>();
    private ResourceNode _closestNode = null;

    public ResourceNode GetClosestNode(List<ResourceNode> nodes)
    {
        _nodes.Remove(_closestNode);
        _buzyNodes.Add(_closestNode);

        foreach (ResourceNode node in nodes)
        {
            if (_nodes.Contains(node) == false && _buzyNodes.Contains(node) == false)
            {
                _nodes.Add(node);
            }
        }

        _closestNode = Utils.GetClosestPosition(transform.position, _nodes);

        return _closestNode;
    }
}
