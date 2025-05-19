using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourcePiece _resourcePiecePrefab;
    [SerializeField] private int _resourceAmount = 2;

    public ResourcePiece Extract(int amount)
    {
        int extracted = Mathf.Min(amount, _resourceAmount);
        _resourceAmount -= extracted;

        if (_resourceAmount <= 0)
        {
            Destroy(gameObject);
        }

        return _resourcePiecePrefab;
    }
}
