using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourceNode[] _resourcePrefabs;
    [SerializeField] private int _itemAmount = 50;
    [SerializeField] private float _recoveryTime = 5f;
    [SerializeField] private int _recoveryCount = 1;

    [SerializeField] private float _spawnRadius = 20f;
    [SerializeField] private float _minDistanceBetweenObjects = 1.5f;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _ignorResourseLayers;

    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_recoveryTime);
        SpawnNode(_itemAmount);
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            yield return _wait;
            SpawnNode(_recoveryCount);
        }
    }

    private void SpawnNode(int itemAmount)
    {
        int spawned = 0;
        int maxAttempts = 500;
        int attempts = 0;

        while (spawned < itemAmount && attempts < maxAttempts)
        {
            Vector3 spawnPos;

            if (TryGetRandomPosition(out spawnPos) && IsValidSpawnPosition(spawnPos))
            {
                ResourceNode resourcePrefab = _resourcePrefabs[Random.Range(0, _resourcePrefabs.Length)];
                Instantiate(resourcePrefab, spawnPos, Quaternion.identity);
                spawned++;
            }

            attempts++;
        }
    }

    private bool TryGetRandomPosition(out Vector3 position)
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        Vector3 startPos = new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y , 
            transform.position.z + randomCircle.y);
       
        if (Physics.Raycast(startPos, Vector3.down, out RaycastHit hit, transform.position.y, _groundLayer))
        {
            position = hit.point;
            return true;
        }
        else
        {
            position = Vector3.zero;
            return false;
        }
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _minDistanceBetweenObjects, _ignorResourseLayers);
        return colliders.Length == 0;
    }
}
