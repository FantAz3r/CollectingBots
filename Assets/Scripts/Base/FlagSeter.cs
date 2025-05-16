using System;
using UnityEngine;

public class FlagSeter : MonoBehaviour
{
    [SerializeField] private Flag flag;

    public event Action<Vector3> FlagSeted;

    public void SetFlag(Vector3 position)
    {
        Instantiate(flag, position, Quaternion.identity);
        FlagSeted?.Invoke(position);
    }

    public void RemoveFlag()
    {
        Destroy(flag.gameObject);
    }
}
