using UnityEngine;

public class ResourcePiece : MonoBehaviour
{
    [SerializeField] private ResourceType _pieceType;

    public ResourceType PeiceType => _pieceType;

    public void PickUp(Transform parent, float holdDistance)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0f, holdDistance);
    }
}
