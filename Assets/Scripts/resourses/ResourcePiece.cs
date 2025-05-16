using UnityEngine;

public class ResourcePiece : MonoBehaviour
{
    [SerializeField] private ResourceType _pieceType;

    public ResourceType PeiceType => _pieceType;
}
