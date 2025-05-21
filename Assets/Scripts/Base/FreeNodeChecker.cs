using System.Collections.Generic;
using UnityEngine;

public class FreeNodeChecker : MonoBehaviour
{
    private List<ResourceNode> _buzyNodes = new List<ResourceNode>();

    public bool IsNodeFree(ResourceNode node)
    {
        _buzyNodes.RemoveAll(node => node == null);

        if (_buzyNodes.Contains(node))
        {
            return false;
        }

        return true;
    }

    public void BuzyNode(ResourceNode node)
    {
        _buzyNodes.Add(node);
    }
}