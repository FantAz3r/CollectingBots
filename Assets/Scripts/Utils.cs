using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static ResourceNode GetClosestPosition(Vector3 currentPosition, List<ResourceNode> resources)
    {
        if (resources == null || resources.Count == 0)
        {
            return null; 
        }

        ResourceNode closestResource = resources[0];
        float minDistance = Vector3.SqrMagnitude(currentPosition - closestResource.transform.position);

        foreach (var resource in resources)
        {
            float dist = Vector3.SqrMagnitude(currentPosition - resource.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closestResource = resource;
            }
        }

        return closestResource;
    }

    public static float SqrDistance(this Vector3 start, Vector3 end)
    {
        return (end - start).sqrMagnitude;
    }

    public static bool IsEnoughClose(this Vector3 start, Vector3 end, float distance)
    {
        return start.SqrDistance(end) <= distance * distance;
    }
}

