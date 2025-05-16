using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(ObjectPicker))]
public class Bot : MonoBehaviour
{
    private Mover _mover;
    private ObjectPicker _picker;
    private Rotator _rotator;
    private Vector3 _basePosition;

    public event Action<Bot, ResourcePiece, int> Returned;
    public event Action<Bot> WorkEnded;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _picker = GetComponent<ObjectPicker>();
        _rotator = GetComponent<Rotator>();
    }

    public void SetBase(Vector3 basePosition)
    {
        _basePosition = basePosition;
    }
    public void StartWork(ResourceNode resource)
    {
        StartCoroutine(Gathering(resource));
    }

    public IEnumerator Gathering(ResourceNode resource)
    {
        while (resource != null)
        {
            _picker.Arrived += Discharge;
            yield return MoveToTarget(resource.transform.position);
            yield return _picker.PickUp(resource);
            yield return MoveToTarget(_basePosition);
        }

        WorkEnded?.Invoke(this);
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        yield return _rotator.Rotate(target);
        yield return _mover.Move(target);
    }

    private void Discharge(ResourcePiece resource, int amount)
    {
        Returned?.Invoke(this, resource, amount);
        _picker.Arrived -= Discharge;
    }
}

