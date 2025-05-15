using UnityEngine;

public class BotScaner : MonoBehaviour
{
    [SerializeField] private float _takeDistance = 5f;
    [SerializeField] private LayerMask _resourseLayer;

    public ResourceNode Scan(Vector3 target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _takeDistance, _resourseLayer);
        
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out ResourceNode resource))
            {
                if (resource.transform.position == target)
                {
                    return resource;
                }
            }
        }

        return null;
    }
}
