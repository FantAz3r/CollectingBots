using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 100f;
    [SerializeField] private float _repeateTime = 5f;
    [SerializeField] private LayerMask _scanLayerMask;

    private WaitForSeconds _wait;

    public event Action<List<ResourceNode>> ScanComplited;

    private void Awake()
    {
        _wait = new WaitForSeconds(_repeateTime);
    }

    private void OnEnable()
    {
        StartCoroutine(Scaning());
    }

    private IEnumerator Scaning()
    {
        while (enabled)
        {
            Scan();
            yield return _wait;
        }
    }

    private void Scan()
    {
        List<ResourceNode> foundResources = new List<ResourceNode>();
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanRadius, _scanLayerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ResourceNode resource))
            {
                foundResources.Add(resource);
            }
        }

        if (foundResources.Count > 0)
        {
            foundResources = SortedByDistance(foundResources);
            ScanComplited?.Invoke(foundResources);
        }
    }

    private List<ResourceNode> SortedByDistance(List<ResourceNode> foundResources)
    {
        return foundResources.OrderBy(resource => (transform.position - resource.transform.position).sqrMagnitude).ToList();
    }
}