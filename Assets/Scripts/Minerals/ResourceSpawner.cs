using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourceNode[] _resourcePrefabs;
    [SerializeField] private int _itemAmount = 50;
    [SerializeField] private float _recoveryTime = 5f;
    [SerializeField] private int _recoveryCount = 1;

    [SerializeField] private float _spawnRadius = 20f;
    [SerializeField] private float _minDistanceBetweenResources = 1.5f;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _resourceLayer;

    private WaitForSeconds _wait;

    private void Start()
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
            Vector3 spawnPos = GetRandomPosition();

            if (IsValidSpawnPosition(spawnPos))
            {
                ResourceNode resourcePrefab = _resourcePrefabs[Random.Range(0, _resourcePrefabs.Length)];
                Instantiate(resourcePrefab, spawnPos, Quaternion.identity);
                spawned++;
            }

            attempts++;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        Vector3 position = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
        RaycastHit hit;
        float offsetY = 0;

        if (Physics.Raycast(position, Vector3.down, out hit, float.MaxValue, _groundLayer))
        {
            position.y = hit.point.y + offsetY;
        }
        else
        {
            position.y = transform.position.y + offsetY;
        }

        return position;
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _minDistanceBetweenResources, _resourceLayer);
        return colliders.Length == 0;
    }
}
