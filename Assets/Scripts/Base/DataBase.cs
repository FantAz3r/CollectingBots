using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private List<ResourceNode> _buzyNodes = new List<ResourceNode>();
    private List<ResourceNode> _nodes = new List<ResourceNode>();

    public ResourceNode SetValidNode(List<ResourceNode> nodes)
    {
        _nodes = nodes;

        if (_buzyNodes.Count > 0)
        {
            foreach (var resource in _buzyNodes)
            {
                _nodes.Remove(resource);
            }
        }

        return Utils.GetClosestPosition(transform.position, _nodes);
    }

    public void SetBuzyNode(ResourceNode node)
    {
        if (node != null)
        {
            _buzyNodes.Add(node);
        }
    }
}
