using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    private List<ResourceNode> _buzyNodes = new List<ResourceNode>();
    private List<ResourceNode> _nodes = new List<ResourceNode>();
    private Dictionary<ResourceNode, Base> _resourceLocks = new Dictionary<ResourceNode, Base>();
    private ResourceNode _currentNode = null;

    public ResourceNode GetFreeNode(List<ResourceNode> nodes, Base currentBase)
    {
        foreach (var node in nodes)
        {
            if (node != null && _nodes.Contains(node) == false && _buzyNodes.Contains(node) == false)
            {
                _nodes.Add(node);
            }
        }

        _nodes.RemoveAll(node => node == null || _buzyNodes.Contains(node));

        if (_nodes.Count == 0)
        {
            _currentNode = null;
            return null;
        }

        _currentNode = Utils.GetClosestNode(currentBase.transform.position, _nodes);

        return _currentNode;
    }

    public void BuzyNode(ResourceNode node, Base currentbase)
    {
        _buzyNodes.Add(node);
        _nodes.Remove(node);
    }

    public bool RequestAccess(Base requestingBase, ResourceNode resource)
    {
        if (_resourceLocks.TryGetValue(resource, out Base lockingBase))
        {
            if (lockingBase != requestingBase)
                return false;
        }

        _resourceLocks[resource] = requestingBase;
        return true;
    }
}
