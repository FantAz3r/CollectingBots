using System;
using System.Collections;
using UnityEngine;

public class FlagSeter : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private Color _baseColor;
    private bool _isSet = false;
    private Vector3 _setPosition = Vector3.zero;
    private Coroutine _waitCoroutine; 

    public event Action<Flag> Seted;
    public bool IsSet => _isSet;

    private void Awake()
    {
        _flag = Instantiate(_flag, transform.position, Quaternion.identity);
        Remove();
        _baseColor = UnityEngine.Random.ColorHSV();
    }

    public void StartWaitForSet()
    {
        _waitCoroutine = StartCoroutine(Wait());  
    }

    public void StopWaitForSet()
    {
        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);  
            _waitCoroutine = null;          
        }
    }

    public void SetPosition(Vector3 position)
    {
        _setPosition = position;
    }

    private IEnumerator Wait()
    {
        while (_setPosition == Vector3.zero)
        {
            yield return null;
        }

        Set(_setPosition, _baseColor);
    }

    private void Set(Vector3 position, Color color)
    {
        if (_isSet == false)
        {
            _isSet = true;
            _flag.SetColor(color);
            _flag.transform.position = position;
            _flag.gameObject.SetActive(true);
            Seted?.Invoke(_flag);
        }
    }

    public void Remove()
    {
        _isSet = false;
        _flag.gameObject.SetActive(false);
        _flag.transform.position = transform.position;
        _setPosition = Vector3.zero;
    }
}
