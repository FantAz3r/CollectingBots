using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private BuildingObject _buildingObject;
    
    public BuildingObject BuildingObject => _buildingObject;

    public GameObject Build(Vector3 place)
    {
        return Instantiate(_buildingObject.Prefab, place, Quaternion.identity);
    }
}
