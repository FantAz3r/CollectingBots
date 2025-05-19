using UnityEngine;

[RequireComponent(typeof(FlagSeter))]
public class Selectable : MonoBehaviour
{
    private FlagSeter _flagSeter;

    private void OnEnable()
    {
        _flagSeter = GetComponent<FlagSeter>();
    }

    public void Select()
    {
        _flagSeter.StartWaitForSet();
    }

    public void Deselect()
    {
        _flagSeter.StopWaitForSet();
    }
}