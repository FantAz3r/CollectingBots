using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(ObjectPicker))]
[RequireComponent(typeof(Builder))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Vector3 _basePosition;

    private Mover _mover;
    private ObjectPicker _picker;
    private Rotator _rotator;
    private Builder _builder;
    private Garage _currentGarage;
    private Coroutine _coroutine;

    private bool _stopGather;

    public event Action<Bot, ResourcePiece, int> Returned;
    public event Action<Bot> WorkEnded;
    public event Action BuildStarted;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _picker = GetComponent<ObjectPicker>();
        _rotator = GetComponent<Rotator>();
        _builder = GetComponent<Builder>();
    }

    public void SetBase(Vector3 basePosition, Garage garage)
    {
        _basePosition = basePosition;
        _currentGarage = garage;
    }

    public void GoToFlag(Vector3 newBasePosition)
    {
        StartCoroutine(Build(newBasePosition));
    }

    public void StopGather(bool stopGather)
    {
        _stopGather = stopGather;
    }

    public void StartWork(ResourceNode resource, Transform garage)
    {
        if (garage == _currentGarage.transform)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(Gathering(resource));
        }
    }

    public IEnumerator Gathering(ResourceNode resource)
    {
        while (resource != null)
        {
            _picker.Arrived += Discharge;
            yield return MoveToTarget(resource.transform.position);
            yield return _picker.PickUp(resource);
            yield return MoveToTarget(_basePosition);

            if(_stopGather)
            {
               break;
            }
        }

        _coroutine = null;
        WorkEnded?.Invoke(this);
    }

    private IEnumerator Build(Vector3 position)
    {
        yield return MoveToTarget(position);

        BuildStarted?.Invoke();
        GameObject newBase = _builder.Build(position);

        if (newBase.TryGetComponent(out Garage garage))
        {
            SetBase(garage.transform.position, garage);
            garage.Return(this);
        }

        BuildStarted = null;
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